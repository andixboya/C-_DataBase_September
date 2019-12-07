

namespace TeisterMask.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class EmployeeImportDto
    {

        public EmployeeImportDto()
        {
            this.Tasks = new List<int>();
        }
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

        [JsonProperty("Tasks")]
        public List<int> Tasks { get; set; }

    }
}
