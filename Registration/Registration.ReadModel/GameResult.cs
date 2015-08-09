using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
    public class GameResult
    {
        public GameResult()
        {
            this.Scores = new List<Shot>();
        }

        public Guid GameId { get; set; }
        public List<Shot> Scores { get; set; }
        public int TotalScore { get; set; }
    }
}
