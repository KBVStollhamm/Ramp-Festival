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
	public class ContestCommandHandler
		: Consumes<RegisterPlayerToContest>.All
	{
		private readonly IEventSourcedRepository<Contest> _repository;

		public ContestCommandHandler(IEventSourcedRepository<Contest> repository)
		{
			_repository = repository;
		}

		public void Consume(RegisterPlayerToContest message)
		{
			if (message == null) throw new ArgumentNullException("message");
			
			Console.WriteLine(message.PlayerName);

			var contest = _repository.Find(message.ContestId);
			if (contest == null)
			{
				contest = new Contest(message.ContestId, message.PlayerName);
			}

			contest.RegisterPlayer(message.PlayerName);

			_repository.Save(contest, message.Id.ToString());
		}
	}
}
