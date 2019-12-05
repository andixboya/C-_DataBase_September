
namespace MusicHub.DataProcessor.ImportDtos
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class ProducerAndAlbumsImportDto
    {
      
        [Required]
        [MinLength(3)]
        [StringLength(30)]
        public string Name { get; set; }

        [RegularExpression("^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string Pseudonym { get; set; }

        [RegularExpression(@"^\+359 \d{3} \d{3} \d{3}$")]
        
        public string PhoneNumber { get; set; }

        
        public List<AlbumExportDto> Albums { get; set; }
    }
    public class AlbumExportDto
    {
        [Required]
        [MinLength(3)]
        [StringLength(40)]
        public string Name { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

    }
}
