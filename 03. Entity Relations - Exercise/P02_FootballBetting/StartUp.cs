using P02_FootballBetting.Data;

namespace P02_FootballBetting
{
    public class StartUp
    {
        private static void Main(string[] args)
        {
            var dbContext = new FootballBettingContext();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}