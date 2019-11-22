

namespace ProductShop.Dtos.Import
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("CategoryProduct")]
    public class ImportCategoryProductDto
    {
        [XmlElement]
        public int CategoryId { get; set; }
        [XmlElement]
        public int ProductId { get; set; }
    }
}
