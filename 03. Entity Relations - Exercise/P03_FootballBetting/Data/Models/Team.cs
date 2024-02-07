using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            Players = new HashSet<Player>();
            AwayTeams = new HashSet<Game>();
            HomeTeams = new HashSet<Game>();
        }

        public int TeamId { get; set; }

        public string Name { get; set; }

        public string LogoUrl { get; set; }

        public Initials Initials { get; set; }

        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }

        [InverseProperty("PrimaryTeamColors")]
        public Color PrimaryKitColor { get; set; }

        public int? SecondaryKitColorId { get; set; }

        [InverseProperty("SecondaryTeamColors")]
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Game> AwayTeams { get; set; }

        public virtual ICollection<Game> HomeTeams { get; set; }
    }
}
