
using System.Xml;

namespace CarDealer.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    
    public class Part
    {
        
        public Part()
        {
            this.PartCars = new HashSet<PartCar>();    
        }

        [XmlAttribute("part")]
        public int Id { get; set; }
        [XmlIgnore]
        public string Name { get; set; }
        [XmlIgnore]
        public decimal Price { get; set; }
        [XmlIgnore]
        public int Quantity { get; set; }
        [XmlIgnore]
        public int SupplierId { get; set; }
        [XmlIgnore]
        public Supplier Supplier { get; set; }
        [XmlIgnore]
        public ICollection<PartCar> PartCars { get; set; }
    }
}
