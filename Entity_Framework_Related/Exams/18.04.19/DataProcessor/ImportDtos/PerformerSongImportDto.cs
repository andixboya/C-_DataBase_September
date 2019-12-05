using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ImportDtos
{
    [XmlType("Performer")]
    public class PerformerSongImportDto
    {

        public PerformerSongImportDto()
        {
            this.PerformersSongs = new List<SingleSong>();
        }

        [XmlElement("FirstName")]
        [Required]
        [MinLength(3)]
        [StringLength(20)]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        [Required]
        [MinLength(3)]
        [StringLength(20)]
        public string LastName { get; set; }

        [XmlElement("Age")]
        [Required]
        [Range(typeof(int), "18", "70")]
        public int Age { get; set; }

        [XmlElement("NetWorth")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [Required]
        public decimal NetWorth { get; set; }

        [XmlArray("PerformersSongs")]
        public List<SingleSong> PerformersSongs { get; set; }
    }

    [XmlType("Song")]
    public class SingleSong
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
