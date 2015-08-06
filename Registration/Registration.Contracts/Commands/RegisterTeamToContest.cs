using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Messaging;

namespace Registration.Commands
{
    public class RegisterTeamToContest : ICommand
    {
        public RegisterTeamToContest()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid ContestId { get; set; }
        public string TeamName { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string Player3Name { get; set; }
        public string Player4Name { get; set; }
        public string Player5Name { get; set; }
    }
}
