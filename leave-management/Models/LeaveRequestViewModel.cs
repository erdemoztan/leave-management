using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Models
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Requesting Employee")]
        public EmployeeViewModel RequestingEmployee { get; set; }

        public string RequestingEmployeeId { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leave Type")]
        public LeaveTypeViewModel LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        [Display(Name = "Date Requested")]
        [DataType(DataType.Date)]
        public DateTime DateRequested { get; set; }

        [Display(Name = "Date of Decision")]
        [DataType(DataType.Date)]
        public DateTime DateActioned { get; set; }

        public bool? Approved { get; set; }

        [Display(Name = "Approved By")]
        public EmployeeViewModel ApprovedBy { get; set; }

        public string ApprovedById { get; set; }
    }

    public class AdminLeaveRequestViewModel
    {
        [Display(Name = "Total Requests")]
        public int TotalRequest { get; set; }

        [Display(Name = "Approved Requests")]
        public int ApprovedRequest { get; set; }

        [Display(Name = "Pending Requests")]
        public int PendingRequest { get; set; }

        [Display(Name = "Rejected Requests")]
        public int RejectedRequest { get; set; }

        public List<LeaveRequestViewModel> LeaveRequests { get; set;  }
    }

    public class CreateLeaveRequestViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leave Type")]
        public LeaveTypeViewModel LeaveType { get; set; }

        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
    }

    public class EmployeeLeaveRequestViewModel
    {
        public List<LeaveRequestViewModel> LeaveRequests { get; set; }

        public List<LeaveAllocationViewModel> LeaveAllocations { get; set; }
    }
}
