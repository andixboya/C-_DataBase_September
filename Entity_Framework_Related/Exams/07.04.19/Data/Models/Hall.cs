

namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    public class Hall
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [MinLength(3)]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        public List<Projection> Projections { get; set; }

        public List<Seat> Seats { get; set; }
    }
}
