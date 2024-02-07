using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class Color
    {
        public Color()
        {
            PrimaryTeamColors = new HashSet<Team>();
            SecondaryTeamColors = new HashSet<Team>();
        }

        public int ColorId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Team> PrimaryTeamColors { get; set; }

        public virtual ICollection<Team> SecondaryTeamColors { get; set; }
    }
}
