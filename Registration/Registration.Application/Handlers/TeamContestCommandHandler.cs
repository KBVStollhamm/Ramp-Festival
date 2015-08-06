using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;
using MassTransit;
using Registration.Commands;
using Registration.Domain.Contest;

namespace Registration.Application.Handlers
{
    public class TeamGameCommandHandler
        : Consumes<RegisterTeamToContest>.All
    {
        private readonly IEventSourcedRepository<TeamGame> _repository;

        public TeamGameCommandHandler(IEventSourcedRepository<TeamGame> repository)
        {
            _repository = repository;
        }

        public void Consume(RegisterTeamToContest message)
        {
            if (message == null) throw new ArgumentNullException("message");

            Console.WriteLine(message.TeamName);

            var game = _repository.Find(message.ContestId);
            if (game == null)
            {
                game = new TeamGame(message.GameId, message.ContestId, message.TeamName, message.Player1Name, message.Player2Name, message.Player3Name, message.Player4Name, message.Player5Name);
            }
            else
            {
                //TODO: Implement update
            }

            _repository.Save(game, message.Id.ToString());
        }
    }

}
