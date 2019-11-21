
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [XmlType("Part")]
    public class ImportPartDto
    { 
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public string Price { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("supplierId")]
        public int SupplierId{ get; set; }


        //    <Part>
        //<name>Bonnet/hood</name>
        //<price>1001.34</price>
        //<quantity>10</quantity>
        //<supplierId>17</supplierId>
        //</Part>
    }
}
