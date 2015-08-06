using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
	public class SinglePlayerGame : EventSourced
	{
        protected SinglePlayerGame(Guid id)
			: base(id)
		{
			this.Handles<SinglePlayerGamePlaced>(When);
		}

		public SinglePlayerGame(Guid id, IEnumerable<IVersionedEvent> history)
			: this(id)
		{
			this.LoadFrom(history);
		}

        public SinglePlayerGame(Guid id, Guid contestId, string playerName) : this(id)
        {
            this.Apply(new SinglePlayerGamePlaced
            {
                ContestId = contestId,
                RegisteredAt = DateTime.Now,
                PlayerName = playerName
            });
        }

		private void When(SinglePlayerGamePlaced e)
		{
		}
	}
}
