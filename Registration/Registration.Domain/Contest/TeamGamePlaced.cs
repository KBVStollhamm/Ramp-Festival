using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
    public class TeamGamePlaced : VersionedEvent
    {
        public Guid ContestId { get; set; }
        public DateTime RegisteredAt { get; set; }

        public string TeamName { get; set; }

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string Player3Name { get; set; }
        public string Player4Name { get; set; }
        public string Player5Name { get; set; }
    }
}
