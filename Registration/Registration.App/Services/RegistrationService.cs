using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registration.Models;
using MassTransit;
using Registration.Commands;

namespace Registration.Services
{
	public class RegistrationService : IRegistrationService
	{
		private readonly IServiceBus _commandBus;

		public RegistrationService(IServiceBus commandBus)
		{
			_commandBus = commandBus;
		}

		public async Task Submit(PlayerContestRegistration registration)
		{
			var command = new RegisterPlayerToContest()
			{
				ContestId = registration.ContestId,
                GameId = Guid.NewGuid(),
                PlayerName = registration.PlayerName
			};

			_commandBus.Publish(command);

			await Task.FromResult<object>(null);
		}

        public async Task Submit(TeamContestRegistration registration)
        {
            var command = new RegisterTeamToContest()
            {
                ContestId = registration.ContestId,
                GameId = Guid.NewGuid(),
                TeamName = registration.TeamName,
                Player1Name = registration.Player1Name,
                Player2Name = registration.Player2Name,
                Player3Name = registration.Player3Name,
                Player4Name = registration.Player4Name,
                Player5Name = registration.Player5Name
            };

            _commandBus.Publish(command);

            await Task.FromResult<object>(null);
        }
    }
}
