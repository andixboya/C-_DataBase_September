
namespace MusicHub.DataProcessor.ImportDtos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class WriterImportDto
    {
        [Required]
        [MinLength(3)]
        [StringLength(20)]
        public string Name { get; set; }


        [RegularExpression("^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string Pseudonym { get; set; }
    }
}
