using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Messaging;

namespace Registration.Commands
{
	public class RegisterPlayerToContest : ICommand
	{
		public RegisterPlayerToContest()
		{
			this.Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
		public Guid GameId { get; set; }
		public Guid ContestId { get; set; }
		public string PlayerName { get; set; }
	}
}
