using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
    public class PlayerScoreUpdated : VersionedEvent
    {
		public string PlayerName { get; set; }
        public int ShotNumber { get; set; }
        public int Points { get; set; }
    }
}
