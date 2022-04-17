using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace EmployeeManagement.Models
{
    public class EmployeeLeave
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Employee ID")]
        public int EmployeeId { get; set; }
        [Required]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Start Date Required")]
        [DisplayName("StartDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date Required")]
        [DisplayName("EndDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Leave Type")]
        public string LeaveType { get; set; }
        [Required(ErrorMessage = "Submitted Date Required")]
        [DisplayName("Submitted Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RequestDate { get; set; }
        [Display(Name = "Approved Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ApprovedDate { get; set; }
        public bool? IsApproved { get; set; }
        [Display(Name = "Approved By")]
        public int? ApprovedBy { get; set; }
        public string Approved_By { get; set; }
        public bool? IsCancelled { get; set; }
        [Required]
        [Display(Name = "Comments")]
        public string Comments { get; set; }

        public virtual EmployeeDetails Employee { get; set; }
    }

    public enum LeaveTypeEnum
    {
        [Display(Name = "Personal Leave")]
        PersonalLeave,

        [Display(Name = "Sick Leave")]
        SickLeave,

        [Display(Name = "Compensation Leave")]
        CompensationLeave,

        [Display(Name = "Annual Leave")]
        AnnualLeave,

        [Display(Name = "Maternity Leave")]
        MaternityLeave,

        [Display(Name = "Paternity Leave")]
        PaternityLeave,
    }
}
