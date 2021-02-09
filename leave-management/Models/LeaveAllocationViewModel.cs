using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Models
{
    public class LeaveAllocationViewModel
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 25, ErrorMessage = "Number of days must be between 0-25")]
        [Display(Name = "Number of Days")]
        public int NumberofDays { get; set; }

        public DateTime DateCreated { get; set; }

        public EmployeeViewModel Employee { get; set; }

        public string EmployeeId { get; set; }

        public LeaveTypeViewModel LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        public int LeaveYear { get; set; }
    }

    public class ViewLeaveAllocationsViewModel
    {
        public EmployeeViewModel Employee { get; set; }

        public string EmployeeId { get; set; }

        public List<LeaveAllocationViewModel> LeaveAllocations { get; set; }
    }
   
}
