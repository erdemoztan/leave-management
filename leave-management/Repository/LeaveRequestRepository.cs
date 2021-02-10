using leave_management.Contracts;
using leave_management.Data;
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
            return _db.LeaveRequests.ToList();
        }

        public LeaveRequest FindById(string id)
        {
            int idInt;

            if (!int.TryParse(id, out idInt))
            {
                return null;
            }

            return _db.LeaveRequests.Find(idInt);
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
    }
}
