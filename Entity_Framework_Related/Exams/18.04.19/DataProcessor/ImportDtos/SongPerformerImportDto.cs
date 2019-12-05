

namespace MusicHub.DataProcessor.ImportDtos
{
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class SongPerformerImportDto
    {
        //<Song>
        //  <Name>What Goes Around</Name>
        //  <Duration>00:03:23</Duration>
        //  <CreatedOn>21/12/2018</CreatedOn>
        //  <Genre>Blues</Genre>
        //  <AlbumId>2</AlbumId>
        //  <WriterId>2</WriterId>
        //  <Price>12</Price>
        //</Song>


        [Required]
        [MinLength(3)]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        public string CreatedOn { get; set; }

        [Required]
        public string Genre { get; set; }
        public int AlbumId { get; set; }
     
        [Required]
        public int WriterId { get; set; }
      

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

    }
}
