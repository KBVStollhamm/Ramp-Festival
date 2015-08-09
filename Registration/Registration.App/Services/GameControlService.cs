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

        public async Task MakePlayerShot(Guid gameId, int shotNumber, int scores)
        {
            var command = new MakePlayerShot
            {
                GameId = gameId,
                ShotNumber = shotNumber,
                Score = scores
            };

            _commandBus.Publish(command);

            await Task.FromResult<object>(null);
        }

        public Task MakeTeamPlayerShot(Guid gameId, string playerName, int shotNumber, int scores)
        {
            throw new NotImplementedException();
        }

        public Task StartSinglePlayerGame(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public Task StartTeamGame(Guid gameId, string playerName)
        {
            throw new NotImplementedException();
        }
    }
}
