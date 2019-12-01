

namespace Cinema.DataProcessor.ImportDto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    //  <Projections>
    //<Projection>
    //  <MovieId>38</MovieId>
    //  <HallId>4</HallId>
    //  <DateTime>2019-04-27 13:33:20</DateTime>
    //</Projection>

    [XmlType("Projection")]
    public class ProjectionImportDto
    {
        [XmlElement("MovieId")]
        public int MovieId { get; set; }
        [XmlElement("HallId")]
        public int HallId { get; set; }

        [Required]
        [XmlElement("DateTime")]
        public string DateTime { get; set; }
    }
}
