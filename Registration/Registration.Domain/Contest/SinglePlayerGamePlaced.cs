using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
	public class SinglePlayerGamePlaced : VersionedEvent
	{
		public Guid ContestId { get; set; }
		public DateTime RegisteredAt { get; set; }

		public string PlayerName { get; set; }
	}
}
