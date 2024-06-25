using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        public Player()
        {
            PlayersStatistics = new HashSet<PlayerStatistic>();
        }

        [Key]
        public int PlayerId { get; set; }

        public string Name { get; set; }

        public int SquadNumber { get; set; }

        public bool IsInjured { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        public ICollection<PlayerStatistic> PlayersStatistics { get; set; }
    }
}
