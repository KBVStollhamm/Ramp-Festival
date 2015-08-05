using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
	public class Contest : EventSourced
	{
		protected Contest(Guid id)
			: base(id)
		{
			this.Handles<ContestPlaced>(When);
			this.Handles<PlayerRegistered>(When);
		}

		public Contest(Guid id, IEnumerable<IVersionedEvent> history)
			: this(id)
		{
			this.LoadFrom(history);
		}

		public Contest(Guid id, string playerName)
			: this(id)
		{
			this.Apply(new ContestPlaced
				{
				});

		}

		public void RegisterPlayer(string playerName)
		{
			this.Apply(new PlayerRegistered
				{
					PlayerName = playerName
				});
		}

		private void When(ContestPlaced e)
		{
		}

		private void When(PlayerRegistered e)
		{
		}
	}
}
