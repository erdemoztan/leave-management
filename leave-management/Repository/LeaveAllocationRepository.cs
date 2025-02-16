﻿using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public bool CheckAllocation(int leaveTypeId, string employeeId, int year)
        {
            return _db.LeaveAllocations.Any(q => q.EmployeeId == employeeId && q.LeaveTypeId == leaveTypeId && q.LeaveYear == year);
        }

        public bool Create(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Add(entity);
            return Save();
        }

        public bool Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return Save();
        }

        public ICollection<LeaveAllocation> FindAll()
        {
            return _db.LeaveAllocations.Include(q => q.LeaveType).Include(q=> q.Employee).ToList();
        }

        public LeaveAllocation FindById(string id)
        {
            int idInt;

            if (!int.TryParse(id, out idInt))
            {
                return null;
            }

            return _db.LeaveAllocations.Include(q=>q.LeaveType).Include(q=>q.Employee).FirstOrDefault(q => q.Id == idInt);
        }

        public IList<LeaveAllocation> GetLeaveAllocationsByEmployeeId(string employeeId, int year)
        {
            return _db.LeaveAllocations.Include(q=> q.LeaveType).Where(q => q.EmployeeId == employeeId && q.LeaveYear == year).ToList();
        }

        public IList<LeaveAllocation> GetLeaveAllocationsByEmployeeIdandLeaveType(string employeeId, int leaveTypeId, int year)
        {
            return _db.LeaveAllocations.Include(q => q.LeaveType).Where(q => q.EmployeeId == employeeId && q.LeaveTypeId == leaveTypeId && q.LeaveYear == year).ToList();
        }

        public bool isExists(int id)
        {
            
            return _db.LeaveAllocations.Any(q => q.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return Save();
        }
    }
}
