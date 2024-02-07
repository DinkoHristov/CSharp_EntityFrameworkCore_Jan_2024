using MusicHub.Data;
using MusicHub.Initializer;
using System;
using System.Linq;
using System.Text;

namespace MusicHub
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new MusicHubDbContext();

            DbInitializer.ResetDatabase(db);

            //string result = ExportAlbumsInfo(db, 9);

            string result = ExportSongsAboveDuration(db, 4);

            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Producers
                .FirstOrDefault(x => x.Id == producerId)
                .Albums
                .Select(a => new
                {
                    a.Name,
                    a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        s.Name,
                        s.Price,
                        SongWriterName = s.Writer.Name
                    })
                    .OrderByDescending(s => s.Name)
                    .ThenBy(s => s.SongWriterName)
                    .ToList(),
                    TotalAlbumPrice = a.Price
                })
                .OrderByDescending(a => a.TotalAlbumPrice)
                .ToList();

            var sb = new StringBuilder();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");

                int count = 1;

                foreach (var song in album.Songs)
                {
                    sb.AppendLine("Songs");
                    sb.AppendLine($"---#{count}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:F2}");
                    sb.AppendLine($"---Writer: {song.SongWriterName}");

                    count++;
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .ToList()
                .Where(s => s.Duration.TotalMinutes > duration)
                .Select(s => new
                {
                    s.Name,
                    PerformerFullName = s.SongPerformers.Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName).FirstOrDefault(),
                    WriterName = s.Writer.Name,
                    ProducerName = s.Album.Producer.Name,
                    s.Duration
                })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ThenBy(s => s.PerformerFullName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var song in songs)
            {
                int count = 1;

                sb.AppendLine($"-Song: #{count}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.WriterName}");
                sb.AppendLine($"---Performer: {song.PerformerFullName}");
                sb.AppendLine($"---Album Producer: {song.ProducerName}");
                sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");

                count++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
