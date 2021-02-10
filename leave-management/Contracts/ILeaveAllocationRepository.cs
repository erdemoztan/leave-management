using leave_management.Contracts;
using leave_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        bool CheckAllocation(int leaveTypeId, string employeeId, int year);

        IList<LeaveAllocation> GetLeaveAllocationsByEmployeeId(string employeeId, int year);

        IList<LeaveAllocation> GetLeaveAllocationsByEmployeeIdandLeaveType(string employeeId, int leaveTypeId,  int year);
    }
}
