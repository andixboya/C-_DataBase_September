

namespace Cinema.DataProcessor.ImportDto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class HallImportDto
    {

        [Required]
        [StringLength(20)]
        [MinLength(3)]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        [Range(typeof(int), "1", "2147483647")]
        public int Seats { get; set; }
    }
}
