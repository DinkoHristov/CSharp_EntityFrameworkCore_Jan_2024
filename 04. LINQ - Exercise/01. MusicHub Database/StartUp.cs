using MusicHub.Data;

namespace MusicHub
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new MusicHubDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
    }
}
