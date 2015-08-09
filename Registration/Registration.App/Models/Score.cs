using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Models
{
    public class Score
    {
        public Score()
        {
        }

        public Score(int shotNumberm, int points)
        {
            this.ShotNumber = ShotNumber;
            this.Points = points;
        }

        public int ShotNumber { get; set; }
        public int Points { get; set; }
    }
}
