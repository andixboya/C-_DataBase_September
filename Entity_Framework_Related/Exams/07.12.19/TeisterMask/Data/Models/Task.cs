

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.Data.Models
{
    public partial class Task
    {

        public Task()
        {
            this.EmployeesTasks = new List<EmployeeTask>();
        }
        [Key]
        public int Id { get; set; }

        [MinLength(2)]
        [MaxLength(40)]
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public ExecutionType ExecutionType { get; set; }

        [Required]
        public LabelType LabelType { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public List<EmployeeTask> EmployeesTasks { get; set; }

    }
}
