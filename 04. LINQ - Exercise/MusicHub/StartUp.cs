namespace MusicHub
{
    using Data;
    using Initializer;
    using System;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here

            //var exportAlbums = ExportAlbumsInfo(context, 9);
            //Console.WriteLine(exportAlbums);

            var songsAboveDuration = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(songsAboveDuration);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .ToList()
                .Select(a => new
                {
                    a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            SongPrice = $"{s.Price:F2}",
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.WriterName)
                        .ToList(),
                    AlbumPrice = a.Price
                })
                .OrderByDescending(a => a.AlbumPrice)
                .ToList();

            var result = new StringBuilder();

            foreach (var album in albums) 
            {
                result.AppendLine($"-AlbumName: {album.Name}");
                result.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                result.AppendLine($"-ProducerName: {album.ProducerName}");

                result.AppendLine("-Songs:");

                var counter = 1;

                foreach (var song in album.Songs)
                {
                    result.AppendLine($"---#{counter}");
                    result.AppendLine($"---SongName: {song.SongName}");
                    result.AppendLine($"---Price: {song.SongPrice}");
                    result.AppendLine($"---Writer: {song.WriterName}");

                    counter++;
                }

                result.AppendLine($"-AlbumPrice: {album.AlbumPrice:F2}");
            }

            return result.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers
                        .Select(p => new
                        {
                            PerformerFullName = $"{p.Performer.FirstName} {p.Performer.LastName}"
                        })
                        .OrderBy(p => p.PerformerFullName)
                        .ToList(),
                    WriterName = s.Writer.Name,
                    AlbumProducerName = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ToList();

            var result = new StringBuilder();

            var counter = 1;

            foreach (var song in songs) 
            {
                result.AppendLine($"-Song #{counter}");
                result.AppendLine($"---SongName: {song.Name}");
                result.AppendLine($"---Writer: {song.WriterName}");

                if (song.Performers.Count > 0)
                {
                    foreach (var performer in song.Performers)
                    {
                        result.AppendLine($"---Performer: {performer.PerformerFullName}");
                    }
                }

                result.AppendLine($"---AlbumProducer: {song.AlbumProducerName}");
                result.AppendLine($"---Duration: {song.Duration}");

                counter++;
            }

            return result.ToString().TrimEnd();
        }
    }
}
