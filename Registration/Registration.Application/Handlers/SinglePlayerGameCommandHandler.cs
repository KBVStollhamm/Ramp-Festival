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
	public class SinglePlayerGameCommandHandler
		: Consumes<RegisterPlayerToContest>.All
	{
		private readonly IEventSourcedRepository<SinglePlayerGame> _repository;

		public SinglePlayerGameCommandHandler(IEventSourcedRepository<SinglePlayerGame> repository)
		{
			_repository = repository;
		}

		public void Consume(RegisterPlayerToContest message)
		{
			if (message == null) throw new ArgumentNullException("message");
			
			Console.WriteLine(message.PlayerName);

			var game = _repository.Find(message.ContestId);
            if (game == null)
            {
                game = new SinglePlayerGame(message.GameId, message.ContestId, message.PlayerName);
            }
            else
            {
                //TODO: Implement update
            }


			_repository.Save(game, message.Id.ToString());
		}
	}
}
