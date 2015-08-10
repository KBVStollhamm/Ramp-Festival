using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Messaging;

namespace ContestManagement.Events
{
	/// <summary>
	/// Base class for contest-related events, containing 
	/// all the contest information.
	/// </summary>
	public abstract class ContestEvent : IEvent
	{
		public Guid SourceId { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public string Slug { get; set; }
		public string Tagline { get; set; }
		public string TwitterSearch { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public ContestType ContestType { get; set; }
	}

	public enum ContestType
	{
		SinglePlayerContest = 1,
		TeamContest = 2,
		ChildrenContest = 3,
		WomenContest = 4
	}
}
