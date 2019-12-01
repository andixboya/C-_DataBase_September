

namespace Cinema.DataProcessor.ImportDto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Customer")]
    public class CustomerTicketImportDto
    {

        [Required]
        [MinLength(3)]
        [StringLength(20)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [StringLength(20)]
        [XmlElement("LastName")]
        public string LastName { get; set; }

        [Required]
        [Range(typeof(int), "12", "110")]
        [XmlElement("Age")]
        public int Age { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement("Balance")]
        public decimal Balance { get; set; }


        [XmlArray("Tickets")]
        public List<TicketView> Tickets { get; set; }


    }

    [XmlType("Ticket")]
    public class TicketView
    {
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        [XmlElement("Price")]
        public decimal Price { get; set; }

        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }
    }
}
