using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
    public class Shot
    {
        public Guid GameId { get; set; }
        public int ShotNumber { get; set; }
        public int Points { get; set; }
    }
}
