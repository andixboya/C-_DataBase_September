

namespace P03_FootballBetting.Data.Models
{
    using System.Collections.Generic;
    using System.Text;
    public class PlayerStatistic
    {

        public int GameId { get; set; }
        
        public Game Game { get; set; }
        
        public int PlayerId { get; set; }
        
        public Player Player { get; set; }

        public int ScoredGoals { get; set; }

        public int Assists { get; set; }

        //TODO: this is probably int, not datetime?
        public int MinutesPlayed { get; set; }

    }
}
