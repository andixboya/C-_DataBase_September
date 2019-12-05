namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var importedWriters = JsonConvert.DeserializeObject<WriterImportDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Writer> writersToAdd = new List<Writer>();
            foreach (var wr in importedWriters)
            {
                if (!IsValid(wr))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                sb.AppendLine(string.Format(SuccessfullyImportedWriter, wr.Name));

                var current = new Writer()
                {
                    Name = wr.Name,
                    Pseudonym = wr.Pseudonym
                };

                writersToAdd.Add(current);
            }

            context.Writers.AddRange(writersToAdd);
            context.SaveChanges();


            var x = sb.ToString().TrimEnd();

            return x;
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var importedPrAndAlb = JsonConvert.DeserializeObject<ProducerAndAlbumsImportDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Producer> producersToAdd = new List<Producer>();

            foreach (var prAndAlbs in importedPrAndAlb)
            {
                bool IsValidProducer = IsValid(prAndAlbs);
                bool areValidAlbs = prAndAlbs.Albums.All(a => IsValid(a));

                if (IsValidProducer is false || areValidAlbs is false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentPro = new Producer()
                {
                    Name = prAndAlbs.Name,
                    PhoneNumber = prAndAlbs.PhoneNumber,
                    Pseudonym = prAndAlbs.Pseudonym,
                };

                currentPro.Albums = prAndAlbs.Albums.Select(a => new Album()
                {
                    Name = a.Name,
                    Producer = currentPro,
                    ReleaseDate = DateTime.ParseExact(a.ReleaseDate, "dd/M/yyyy", CultureInfo.InvariantCulture)
                }).ToList();

                producersToAdd.Add(currentPro);

                if (prAndAlbs.PhoneNumber is null)
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithNoPhone, currentPro.Name, currentPro.Albums.Count));
                }
                else
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithPhone, currentPro.Name, currentPro.PhoneNumber, currentPro.Albums.Count));
                }
            }


            context.Albums.AddRange(producersToAdd.SelectMany(p => p.Albums));
            context.Producers.AddRange(producersToAdd);
            context.SaveChanges();

            var x = sb.ToString().TrimEnd();

            return x;
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var importedSongs = ImportXml<SongPerformerImportDto>(xmlString, "Songs");

            var validAlbumIds = context.Albums.Select(a => a.Id).ToHashSet<int>();
            var validWriterIds = context.Writers.Select(w => w.Id).ToHashSet<int>();


            var songsToAdd = new List<Song>();
            StringBuilder sb = new StringBuilder();

            foreach (var song in importedSongs)
            {
                bool isValidObject = IsValid(song);
                bool isValidGenre = Enum.TryParse(song.Genre, out Genre genreResult);

                if (isValidObject is false ||
                    isValidGenre is false ||
                    !validAlbumIds.Contains(song.AlbumId) ||
                    !validWriterIds.Contains(song.WriterId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var songToAdd = new Song()
                {
                    WriterId = song.WriterId,
                    AlbumId = song.AlbumId,
                    CreatedOn = DateTime.ParseExact(song.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    //questionable??? 
                    Duration = TimeSpan.ParseExact(song.Duration, "c", CultureInfo.InvariantCulture),
                    Genre = genreResult,
                    Name = song.Name,
                    Price = song.Price,
                };
                songsToAdd.Add(songToAdd);

                sb.AppendLine(
                    string.Format(SuccessfullyImportedSong,
                    songToAdd.Name,
                    songToAdd.Genre,
                    songToAdd.Duration.ToString("c")));
            }

            context.Songs.AddRange(songsToAdd);
            context.SaveChanges();

            var x = sb.ToString().TrimEnd();

            return x;
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var importedPerformers = ImportXml<PerformerSongImportDto>(xmlString, "Performers");
            var validSongIds = context.Songs.Select(s => s.Id).ToList();


            var performersToAdd = new List<Performer>();
            StringBuilder sb = new StringBuilder();

            var songBondsToInsert = new List<SongPerformer>();
            foreach (var per in importedPerformers.ToList())
            {

                bool isValidObj = IsValid(per);
                bool validCollections = per.PerformersSongs.All(p => validSongIds.Contains(p.Id));

                if (isValidObj is false || validCollections is false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }



                var performer = new Performer()
                {
                    Age = per.Age,
                    FirstName = per.FirstName,
                    LastName = per.LastName,
                    NetWorth = per.NetWorth,
                };


                foreach (var s in per.PerformersSongs)
                {
                    var current = new SongPerformer()
                    {
                        Performer = performer,
                        SongId = s.Id
                    };
                    songBondsToInsert.Add(current);
                    performer.PerformerSongs.Add(current);
                }

                sb.AppendLine(
                    string.Format(SuccessfullyImportedPerformer,
                    performer.FirstName,
                    performer.PerformerSongs.Count));

            }

            //not sure if i should add the mapping table or this way it is enough? 
            context.Performers.AddRange(performersToAdd);
            context.SongsPerformers.AddRange(songBondsToInsert);
            context.SaveChanges();
            var x = sb.ToString().TrimEnd();

            return x;
        }

        private static T[] ImportXml<T>(string xmlString, string rootName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootName);

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlSerializer serializer = new XmlSerializer(typeof(T[]), root);

            using (var stream = new StringReader(xmlString))
            {
                var importedProjections = (T[])serializer.Deserialize(stream);
                return importedProjections;
            }
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}