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
		, Consumes<StartTeamGame>.All
        , Consumes<MakeTeamPlayerShot>.All
    {
		private readonly IEventSourcedRepository<TeamGame> _repository;

		public TeamGameCommandHandler(IEventSourcedRepository<TeamGame> repository)
		{
			_repository = repository;
		}

        public void Consume(RegisterTeamToContest message)
		{
			try
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
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void Consume(StartTeamGame message)
		{
			try
			{
				if (message == null) throw new ArgumentNullException("message");

				Console.WriteLine("Starting team player game with ID: {0}", message.GameId);

				var game = _repository.Find(message.GameId);
				if (game != null)
				{
					game.Start(message.PlayerName);
					Console.WriteLine("Team player game with ID: {0} started.", message.GameId);
				}
				else
				{
					Console.WriteLine("Couldn't find a team player game with ID: {0}", message.GameId);
				}

				_repository.Save(game, message.Id.ToString());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

        public void Consume(MakeTeamPlayerShot message)
        {
			try
			{ 
            if (message == null) throw new ArgumentNullException("message");

            Console.WriteLine(message.Score);

            var game = _repository.Find(message.GameId);
            if (game != null)
            {
                game.MakeShot(message.PlayerName, message.ShotNumber, message.Score);
            }

            _repository.Save(game, message.Id.ToString());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
        }
    }
}
