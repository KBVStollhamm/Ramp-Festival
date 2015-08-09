using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Events.Live
{
    public class PlayerScored
    {
        public Guid GameId { get; set; }
        public string PlayerName { get; set; }
        public int ShotNumber { get; set; }
        public int Points { get; set; }
    }
}
