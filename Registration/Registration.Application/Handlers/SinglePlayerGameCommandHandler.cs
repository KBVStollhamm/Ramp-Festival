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
		, Consumes<StartSinglePlayerGame>.All
		, Consumes<MakePlayerShot>.All
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

			var game = _repository.Find(message.GameId);
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


		public void Consume(StartSinglePlayerGame message)
		{
			if (message == null) throw new ArgumentNullException("message");

			Console.WriteLine("Starting single player game with ID: {0}", message.GameId);

			var game = _repository.Find(message.GameId);
			if (game != null)
			{
				game.Start();

				_repository.Save(game, message.Id.ToString());
				Console.WriteLine("Single player game with ID: {0} started.", message.GameId);
			}
			else
			{
				Console.WriteLine("Couldn't find a single player game with ID: {0}", message.GameId);
			}
		}

		public void Consume(MakePlayerShot message)
		{
			if (message == null) throw new ArgumentNullException("message");

			Console.WriteLine(message.Score);

			var game = _repository.Find(message.GameId);
			if (game != null)
			{
				game.MakeShot(message.PlayerName, message.ShotNumber, message.Score);

				_repository.Save(game, message.Id.ToString());
			}
		}
	}
}
