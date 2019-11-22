

namespace ProductShop.Dtos.Export
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Users")]
    public class ExportUsersWithProductsDto
    {
        
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public AnotherSingleUser[] SingleUsers { get; set; }
    }

 
    [XmlType("User")]
    public class AnotherSingleUser
    
    {

        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }


        [XmlElement("SoldProducts")]
        public SoldProductsType SoldProduct { get; set; }
    }


    [XmlType("SoldProducts")]
    public class SoldProductsType
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public SingleProduct[] SoldProducts { get; set; }
    }

    [XmlType("Product")]
    public class SingleProduct
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
