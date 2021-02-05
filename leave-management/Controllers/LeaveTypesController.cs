using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ILeaveTypeRepository _repo;
        private readonly IMapper _mapper;

        public LeaveTypesController(ILeaveTypeRepository repo, IMapper mapper)
        {
            this._repo = repo;
            this._mapper = mapper;
        }

        // GET: LeaveTypesController
        public ActionResult Index()
        {
            var leaveTypes = _repo.FindAll();
            var model =_mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveTypes.ToList());
            return View(model);
        }

        // GET: LeaveTypesController/Details/5
        public ActionResult Details(int id)
        {
            var value = _repo.FindById(id.ToString());

            if (value == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<LeaveTypeViewModel>(value);
            return View(model);
        }

        // GET: LeaveTypesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeaveTypeViewModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }

                var leaveType = _mapper.Map<LeaveType>(model);
                leaveType.DateCreated = DateTime.UtcNow;

                if(!_repo.Create(leaveType))
                {
                    ModelState.AddModelError("", "Something went wrong....");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong....");
                return View();
            }
        }

        // GET: LeaveTypesController/Edit/5
        public ActionResult Edit(int id)
        {
            var value = _repo.FindById(id.ToString());
            
            if(value == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<LeaveTypeViewModel>(value);
            return View(model);
        }

        // POST: LeaveTypesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LeaveTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var leaveType = _mapper.Map<LeaveType>(model);
                if(! _repo.Update(leaveType))
                {
                    ModelState.AddModelError("", "Something went wrong....");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong....");
                return View();
            }
        }

        // GET: LeaveTypesController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _repo.FindById(id.ToString());

                if (entity == null)
                {
                    return NotFound();
                }

                if (!_repo.Delete(entity))
                {
                    ModelState.AddModelError("", "Something went wrong....");
                    return BadRequest();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong....");
                return BadRequest();
            }
        }

        // POST: LeaveTypesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LeaveTypeViewModel model)
        {
            try
            {
                var entity = _repo.FindById(id.ToString());

                if(entity == null)
                {
                    return NotFound();
                }

                if (!_repo.Delete(entity))
                {
                    ModelState.AddModelError("", "Something went wrong....");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong....");
                return View(model);
            }
        }
    }
}
