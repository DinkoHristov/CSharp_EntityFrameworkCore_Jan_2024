using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class Game
    {
        public Game()
        {
            PlayerStatistics = new HashSet<PlayerStatistic>();
            Bets = new HashSet<Bet>();
        }

        public int GameId { get; set; }

        public double AwayTeamBetRate { get; set; }

        public int AwayTeamGoals { get; set; }

        public int? AwayTeamId { get; set; }

        [InverseProperty("AwayTeams")]
        public Team AwayTeam { get; set; }

        public double DrawBetRate { get; set; }

        public double HomeTeamBetRate { get; set; }

        public int HomeTeamGoals { get; set; }

        public int? HomeTeamId { get; set; }

        [InverseProperty("HomeTeams")]
        public Team HomeTeam { get; set; }

        public int Result { get; set; }

        public DateTime DateTime { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
