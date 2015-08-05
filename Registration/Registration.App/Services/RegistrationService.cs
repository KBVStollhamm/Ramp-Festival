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
				PlayerName = registration.PlayerName
			};

			_commandBus.Publish(command);

			await Task.FromResult<object>(null);
		}
	}
}
