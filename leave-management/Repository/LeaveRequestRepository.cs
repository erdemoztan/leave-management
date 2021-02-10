using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repository
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRequestRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public bool Create(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return Save();
        }

        public bool Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }

        public ICollection<LeaveRequest> FindAll()
        {
            return _db.LeaveRequests
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q=>q.LeaveType)
                .ToList();
        }

        public LeaveRequest FindById(string id)
        {
            int idInt;

            if (!int.TryParse(id, out idInt))
            {
                return null;
            }

            return _db.LeaveRequests
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .FirstOrDefault(q=> q.Id==idInt);
        }

        public bool Save()
        {
            return _db.SaveChanges()>0;
        }

        public bool Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }

        public bool isExists(int id)
        {

            return _db.LeaveRequests.Any(q => q.Id == id);
        }

        public ICollection<LeaveRequest> GetLeaveRequestsByEmployeeId(string employeeId)
        {
            return _db.LeaveRequests.Where(q => q.RequestingEmployeeId == employeeId).ToList();
        }
    }
}
