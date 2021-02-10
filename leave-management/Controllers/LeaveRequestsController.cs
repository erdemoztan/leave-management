using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
    [Authorize]
    public class LeaveRequestsController : Controller
    {

        private readonly ILeaveRequestRepository _repo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly UserManager<Employee> _userManager;
        private readonly IMapper _mapper;

        private CreateLeaveRequestViewModel GetCreateLeaveRequestVM()
        {
            var leaveTypes = _mapper.Map<IList<LeaveTypeViewModel>>(_leaveTypeRepo.FindAll());
            var leaveTypesModel = leaveTypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            var model = new CreateLeaveRequestViewModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                LeaveTypes = leaveTypesModel
            };

            return model;
        }

        public LeaveRequestsController(
              ILeaveRequestRepository repo
            , ILeaveTypeRepository leaveTypeRepo
            , ILeaveAllocationRepository leaveAllocationRepo
            , UserManager<Employee> userManager
            , IMapper mapper
            )
        {
            this._mapper = mapper;
            this._repo = repo;
            this._userManager = userManager;
            this._leaveTypeRepo = leaveTypeRepo;
            this._leaveAllocationRepo = leaveAllocationRepo;
        }

        // GET: LeaveRequestsController
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            var leaveRequests = _repo.FindAll();
            var leaveRequestVM = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests);

            var model = new AdminLeaveRequestViewModel
            {
                TotalRequest = leaveRequestVM.Count(),
                ApprovedRequest = leaveRequestVM.Count(q => q.Approved == true),
                PendingRequest = leaveRequestVM.Count(q => q.Approved == null),
                RejectedRequest = leaveRequestVM.Count(q => q.Approved == false),
                LeaveRequests = leaveRequestVM
            };

            return View(model);
        }

        // GET: LeaveRequestsController/Details/5
        public ActionResult Details(int id)
        {
            var model = _mapper.Map<LeaveRequestViewModel>(_repo.FindById(id.ToString()));
            return View(model);
        }

        // GET: LeaveRequestsController/Create
        public ActionResult Create()
        {
            var model = GetCreateLeaveRequestVM();

            return View(model);
        }

        public ActionResult ApproveRequest(int id)
        {
            try
            {
                var request = _repo.FindById(id.ToString());
                var approver = _userManager.GetUserAsync(User).Result;
                request.Approved = true;
                request.ApprovedBy = approver;
                request.ApprovedById = approver.Id;
                request.DateActioned = DateTime.UtcNow;

                if (!_repo.Update(request))
                {
                    ModelState.AddModelError("", "No record found.");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "An error occured.");
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult RejectRequest(int id)
        {
            try
            {
                var request = _repo.FindById(id.ToString());
                var allocation = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeIdandLeaveType(request.RequestingEmployeeId, request.LeaveTypeId, DateTime.Now.Year).FirstOrDefault();

                var numberOfDays = (int)(request.EndDate.Date - request.StartDate.Date).TotalDays;
                var approver = _userManager.GetUserAsync(User).Result;
                request.Approved = false;
                request.ApprovedBy = approver;
                request.ApprovedById = approver.Id;
                request.DateActioned = DateTime.UtcNow;
                allocation.NumberofDays += numberOfDays;

                var op1 = _repo.Update(_mapper.Map<LeaveRequest>(request));
                var op2 = _leaveAllocationRepo.Update(allocation);

                if (!op1 || !op2)
                {
                    var errormodel = GetCreateLeaveRequestVM();
                    ModelState.AddModelError("", "Error during save.");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "An error occured.");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: LeaveRequestsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestViewModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    var errormodel = GetCreateLeaveRequestVM();
                    return View(errormodel);

                }

                if(model.StartDate >= model.EndDate)
                {
                    var errormodel = GetCreateLeaveRequestVM();
                    ModelState.AddModelError("", "Start date must be smaller than end date.");
                    return View(errormodel);
                }

                var numberOfDays = (int)(model.EndDate.Date - model.StartDate.Date).TotalDays;


                var employee = _userManager.GetUserAsync(User).Result;
                var allocation = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeIdandLeaveType(employee.Id, model.LeaveTypeId, DateTime.Now.Year).FirstOrDefault();

                if (numberOfDays >  allocation.NumberofDays)
                {
                    var errormodel = GetCreateLeaveRequestVM();
                    ModelState.AddModelError("", "Not enough remaining days.");
                    return View(errormodel);
                }

                allocation.NumberofDays -= numberOfDays;
                var request = new LeaveRequestViewModel()
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    DateRequested = DateTime.UtcNow,
                    Approved = null,
                    DateActioned = DateTime.UtcNow,
                    LeaveTypeId = model.LeaveTypeId
                };

                var op1 = _repo.Create(_mapper.Map<LeaveRequest>(request));
                var op2 = _leaveAllocationRepo.Update(allocation);

                if(!op1 || !op2)
                {
                    var errormodel = GetCreateLeaveRequestVM();
                    ModelState.AddModelError("", "Error during save.");
                    return View(errormodel);
                }

                return RedirectToAction(nameof(Index),"Home");
            }
            catch
            {
                ModelState.AddModelError("", "Operation Error");
                var errormodel = GetCreateLeaveRequestVM();
                return View(errormodel);
            }
        }

        // GET: LeaveRequestsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequestsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveRequestsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequestsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult MyLeave()
        {
            try
            {
                var employee = _userManager.GetUserAsync(User).Result;

                var allocations = _mapper.Map<List<LeaveAllocationViewModel>>(_leaveAllocationRepo.GetLeaveAllocationsByEmployeeId(employee.Id, DateTime.Now.Year));
                var leaveRequests = _mapper.Map<List<LeaveRequestViewModel>>(_repo.GetLeaveRequestsByEmployeeId(employee.Id));

                var model = new EmployeeLeaveRequestViewModel
                {
                    LeaveAllocations = allocations,
                    LeaveRequests = leaveRequests
                };

                return View(model);
            }
            catch
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        public ActionResult CancelRequest(int id)
        {
            try
            {
                var request = _repo.FindById(id.ToString());
                var allocation = _leaveAllocationRepo.GetLeaveAllocationsByEmployeeIdandLeaveType(request.RequestingEmployeeId, request.LeaveTypeId, DateTime.Now.Year).FirstOrDefault();

                var numberOfDays = (int)(request.EndDate.Date - request.StartDate.Date).TotalDays;
                var approver = _userManager.GetUserAsync(User).Result;
                request.Approved = false;
                request.ApprovedBy = approver;
                request.ApprovedById = approver.Id;
                request.DateActioned = DateTime.UtcNow;
                allocation.NumberofDays += numberOfDays;

                var op1 = _repo.Delete(_mapper.Map<LeaveRequest>(request));
                var op2 = _leaveAllocationRepo.Update(allocation);

                if (!op1 || !op2)
                {
                    var errormodel = GetCreateLeaveRequestVM();
                    ModelState.AddModelError("", "Error during save.");
                }
                return RedirectToAction(nameof(MyLeave));
            }
            catch
            {
                ModelState.AddModelError("", "An error occured.");
                return RedirectToAction(nameof(MyLeave));
            }
        }
    }
}
