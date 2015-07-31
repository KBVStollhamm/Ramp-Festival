using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;
using Infrastructure.Messaging.Handling;
using Registration.Commands;
using Registration.Domain;

namespace Registration.Application.Handlers
{
	public class ParticipationCommandHandler : ICommandHandler<RegisterPlayerToContest>
	{
		private readonly IEventSourcedRepository<Participation> _repository;

		public ParticipationCommandHandler(IEventSourcedRepository<Participation> repository)
		{
			if (repository == null) throw new ArgumentNullException("repository");

			_repository = repository;
		}

		public void Handle(RegisterPlayerToContest command)
		{
			if (command == null) throw new ArgumentNullException("command");

			var participation = _repository.Find(command.ParticipationId);
			if (participation == null)
			{
				//TODO: Create paticipation
				participation = new Participation(command.ParticipationId, command.PlayerName)
			}
			else
			{
				//TODO: Update participation
			}

			_repository.Save(participation, command.Id.ToString());
		}
	}
}
