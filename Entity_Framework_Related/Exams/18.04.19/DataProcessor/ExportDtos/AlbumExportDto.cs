

namespace MusicHub.DataProcessor.ExportDtos
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class AlbumExportDto
    {
        public AlbumExportDto()
        {
            this.Songs = new List<SongExport>();
        }
        public string AlbumName { get; set; }

        public string ReleaseDate { get; set; }

        public string ProducerName { get; set; }


        public List<SongExport> Songs { get; set; }

        public string AlbumPrice { get; set; }

    }

    public class SongExport
    {
        public string SongName { get; set; }

        public string Price { get; set; }

        public string Writer { get; set; }

    }
}
