using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            this.EmployeesTasks = new List<EmployeeTask>();
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"(^([A-Z0-9]+)$)|(^([a-z0-9]+)$)")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        public string Phone { get; set; }

        public List<EmployeeTask> EmployeesTasks { get; set; }
    }
}
