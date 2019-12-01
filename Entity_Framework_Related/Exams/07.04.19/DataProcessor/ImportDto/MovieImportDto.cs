

namespace Cinema.DataProcessor.ImportDto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    
    public class MovieImportDto
    {
        public int Id { get; set; }

        [StringLength(20)]
        [MinLength(3)]
        [Required]
        public string Title { get; set; }


        public string Genre { get; set; }

        //this might... be long!?
        public TimeSpan Duration { get; set; }

        [Range(1, 10)]
        public double Rating { get; set; }

        [StringLength(20)]
        [MinLength(3)]
        public string Director { get; set; }
    }
}
