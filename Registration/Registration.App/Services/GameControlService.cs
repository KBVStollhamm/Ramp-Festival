using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Registration.Commands;

namespace Registration.Services
{
	public class GameControlService : IGameControlService
	{
		private readonly IServiceBus _commandBus;

		public GameControlService(IServiceBus commandBus)
		{
			_commandBus = commandBus;
		}

		public async Task MakePlayerShot(Guid gameId, string playerName, int shotNumber, int scores)
		{
			var command = new MakePlayerShot
			{
                PlayerName = playerName,
				GameId = gameId,
				ShotNumber = shotNumber,
				Score = scores
			};

			_commandBus.Publish(command);

			await Task.FromResult<object>(null);
		}

		public async Task MakeTeamPlayerShot(Guid gameId, string playerName, int shotNumber, int scores)
		{
            var command = new MakeTeamPlayerShot
            {
                PlayerName = playerName,
                GameId = gameId,
                ShotNumber = shotNumber,
                Score = scores
            };

            _commandBus.Publish(command);

            await Task.FromResult<object>(null);
        }

		public async Task StartSinglePlayerGame(Guid gameId)
		{
			var command = new StartSinglePlayerGame
			{
				GameId = gameId				
			};

			_commandBus.Publish(command);

			await Task.FromResult<object>(null);
		}

		public async Task StartTeamGame(Guid gameId, string playerName)
		{
			var command = new StartTeamGame
			{
				GameId = gameId,
				PlayerName = playerName
				
			};

			_commandBus.Publish(command);

			await Task.FromResult<object>(null);
		}
	}
}
