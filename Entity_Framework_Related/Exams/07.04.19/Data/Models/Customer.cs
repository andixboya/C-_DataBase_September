using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Tickets = new List<Ticket>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [Range(typeof(int), "12", "110")]
        public int Age { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335M")]
        public decimal Balance { get; set; }

        public List<Ticket> Tickets { get; set; }

    }
}
