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
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leave Type")]
        public LeaveTypeViewModel LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        [Display(Name = "Date Requested")]
        public DateTime DateRequested { get; set; }

        [Display(Name = "Date of Decision")]
        public DateTime DateActioned { get; set; }

        public bool? Approved { get; set; }

        [Display(Name = "Approved By")]
        public EmployeeViewModel ApprovedBy { get; set; }

        public string ApprovedById { get; set; }

        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
    }
}
