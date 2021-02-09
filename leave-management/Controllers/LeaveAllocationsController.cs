using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeaveAllocationsController : Controller
    {
        private readonly ILeaveAllocationRepository _repo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationsController(
            ILeaveAllocationRepository repo
            , ILeaveTypeRepository leaveTypeRepository
            , IMapper mapper
            , UserManager<Employee> userManager)
        {
            this._repo = repo;
            this._mapper = mapper;
            this._leaveTypeRepo = leaveTypeRepository;
            this._userManager = userManager;
        }

        // GET: LeaveAllocationController
        public ActionResult Index(int count = 0)
        {
            var leaveTypes = _leaveTypeRepo.FindAll();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveTypes.ToList());

            var model = new CreateLeaveTypeViewModel
            {
                LeaveTypes = mappedLeaveTypes,
                NumberUpdated = count
            };

            return View(model);
        }

        // GET: LeaveAllocationController/Details/5
        public ActionResult Details(string id)
        {
            var employee = _mapper.Map<EmployeeViewModel>(_userManager.FindByIdAsync(id).Result);
            var leaveAllocations = _mapper.Map<List<LeaveAllocationViewModel>>(_repo.GetLeaveAllocationsByEmployeeId(id, DateTime.Now.Year));

            var model = new ViewLeaveAllocationsViewModel
            {
                Employee = employee,
                EmployeeId = id,
                LeaveAllocations = leaveAllocations
            };

            return View(model);
        }

        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: LeaveAllocationController/Edit/5
        public ActionResult Edit(int id)
        {
            var leaveAllocation = _mapper.Map<LeaveAllocationViewModel>(_repo.FindById(id.ToString()));
            return View(leaveAllocation);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LeaveAllocationViewModel model)
        {
            try
            {
                var leaveAllocation = _mapper.Map<LeaveAllocation>(model);
                if (!_repo.Update(leaveAllocation))
                {
                    ModelState.AddModelError("", "Something went wrong....");
                    return View(model);
                }
                return RedirectToAction(nameof(ListEmployees));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong....");
                return View();
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
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

        public ActionResult SetLeave(int id)
        {
            var count = 0;
            try
            {
                var leaveType = _leaveTypeRepo.FindById(id.ToString());
                var employees = _userManager.GetUsersInRoleAsync("Employee").Result;

                foreach (var item in employees)
                {
                    if (!_repo.CheckAllocation(id, item.Id, DateTime.Now.Year))
                    {
                        var allocation = new LeaveAllocationViewModel
                        {
                            DateCreated = DateTime.UtcNow,
                            EmployeeId = item.Id,
                            LeaveTypeId = id,
                            NumberofDays = leaveType.DefaultDays,
                            LeaveYear = DateTime.Now.Year
                        };

                        var leaveAllocation = _mapper.Map<LeaveAllocation>(allocation);
                        _repo.Create(leaveAllocation);

                        count++;
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error occured, Action may not be fully completed.");
            }

            return RedirectToAction(nameof(Index),new { @count = count });
        }

        public ActionResult ListEmployees()
        {
            var employees = _userManager.GetUsersInRoleAsync("Employee").Result;
            var model = _mapper.Map<List<Employee>, List<EmployeeViewModel>>(employees.ToList());
            return View(model);
        }
    }
}
