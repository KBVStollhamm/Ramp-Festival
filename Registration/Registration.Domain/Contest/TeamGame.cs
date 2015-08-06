using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain.Contest
{
    public class TeamGame : EventSourced
    {
        protected TeamGame(Guid id)
			: base(id)
		{
            this.Handles<TeamGamePlaced>(When);
        }

        public TeamGame(Guid id, IEnumerable<IVersionedEvent> history)
			: this(id)
		{
            this.LoadFrom(history);
        }

        public TeamGame(Guid id, Guid contestId, string teamName, string player1Name, string player2Name, string player3Name, string player4Name, string player5Name):this(id)
        {
            this.Apply(new TeamGamePlaced
            {
                ContestId = contestId,
                RegisteredAt = DateTime.Now,
                TeamName = teamName,
                Player1Name = player1Name,
                Player2Name = player2Name,
                Player3Name = player3Name,
                Player4Name = player4Name,
                Player5Name = player5Name
            });
        }

        private void When(TeamGamePlaced e)
        {
       }
    }
}
