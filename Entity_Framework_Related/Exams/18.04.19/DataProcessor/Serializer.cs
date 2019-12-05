namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums =
                context.
                Albums
                .Where(a => a.ProducerId == producerId)
                .Select(a => new AlbumExportDto()
                {
                    AlbumName = a.Name,
                    ProducerName = a.Producer.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Songs = a.Songs.Select(s => new SongExport
                    {
                        Price = s.Price.ToString("f2"),
                        SongName = s.Name,
                        Writer = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.Writer)
                    .ToList()
                    ,
                    AlbumPrice = a.Songs.Sum(s => s.Price).ToString("f2")
                })
                .OrderByDescending(a => decimal.Parse(a.AlbumPrice))
                .ToList();
            Song song = new Song()
            {
                CreatedOn = DateTime.ParseExact("2018-12-21", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Duration = new TimeSpan(5000),
                Genre = Genre.Blues,
                Name = "HAHA",
                Price = 50.32m,
                WriterId = 5,
                
            };
            context.Songs.Add(song);
            context.SaveChanges();
            ;



            var result = JsonConvert.SerializeObject(albums, Newtonsoft.Json.Formatting.Indented);
            return result;
        }



        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songsToExport =
                context.
                Songs
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new SongExportDto()
                {
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c"),
                    Performer = s.SongPerformers.Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName).FirstOrDefault(),
                    SongName = s.Name,
                    Writer = s.Writer.Name
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.Writer)
                .ThenBy(s => s.Performer)
                .ToArray();

            var result = XmlExport("Songs", songsToExport);

            return result;
        }

        private static string XmlExport<T>(string rootName, T[] collectionToExport)
        {
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName() });

            XmlRootAttribute root = new XmlRootAttribute(rootName);
            var serializer = new XmlSerializer(typeof(T[]), root);

            StringBuilder sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, collectionToExport, namespaces);

            }

            return sb.ToString().TrimEnd();
        }
    }
}