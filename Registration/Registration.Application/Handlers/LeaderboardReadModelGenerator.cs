using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Registration.Domain.Contest;
using Registration.ReadModel.Implementation;

namespace Registration.Application.Handlers
{
	public class LeaderboardReadModelGenerator
		: Consumes<GameFinished>.All
	{
		private readonly Func<RegistrationDbContext> _contextFactory;

		public LeaderboardReadModelGenerator(Func<RegistrationDbContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public void Consume(GameFinished message)
		{
		}
	}
}
