

namespace CarDealer.DTO.Import
{
    using CarDealer.Models;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    //collection
    [DataContract]
    public class CarWithPartsVM
    {

        public CarWithPartsVM()
        {
            this.Car = new SingleCar();
            this.Parts = new List<SinglePart>();
        }

        [DataMember(Name = "car")]
        public SingleCar Car { get; set; }

        [DataMember(Name = "parts")]
        public ICollection<SinglePart> Parts { get; set; }

    }

    public class SingleCar
    {
        public string Make { get; set; }
        public string Model { get; set; }

        public long TravelledDistance { get; set; }
    }

    public class SinglePart
    {
        public string Name { get; set; }
        public string Price { get; set; }
    }


}
