using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Events.Live
{
    public class GameStarted
    {
        public Guid GameId { get; set; }
        public string PlayerName { get; set; }
        public string TeamName { get; set; }
    }
}
