

namespace CarDealer.DTO.Import
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;
    [DataContract]
    public class CarImportDto
    {
        //    "make": "Opel",
        //"model": "Insignia",
        //"travelledDistance": 225253817,
        //"partsId": [

        public CarImportDto()
        {
            this.PartCars = new List<int>();
        }

        public int Id { get; set; }
        [DataMember(Name ="make")]
        public string Make { get; set; }

        [DataMember(Name ="travelledDistance")]
        public long TravelledDistance { get; set; }

        [DataMember(Name ="model")]
        public string Model { get; set; }

        [DataMember(Name = "partsId")]
        public ICollection<int> PartCars { get; set; }
    }
}
