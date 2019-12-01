using Cinema.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Movie
    {

        public Movie()
        {
            this.Projections = new List<Projection>();
        }

        public int Id { get; set; }

        [StringLength(20)]
        [MinLength(3)]
        [Required]
        public string Title { get; set; }
        

        public Genre Genre { get; set; }

        //this might... be long!?
        public TimeSpan Duration { get; set; }

        [Range(1,10)]
        public double Rating { get; set; }

        [StringLength(20)]
        [MinLength(3)]
        public string Director { get; set; }


        public List<Projection> Projections { get; set; }

    }


}
