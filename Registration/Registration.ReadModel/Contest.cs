using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
	public class Contest
	{
		public Contest(Guid id, ContestType contestType, string code, string name, string description, string location, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			this.Id = id;
			this.ContestType = contestType;
			this.Code = code;
			this.Name = name;
			this.Description = description;
			this.Location = location;
			this.StartDate = startDate;
			this.EndDate = endDate;
		}

		protected Contest()
		{
		}

		public Guid Id { get; set; }
		public ContestType ContestType { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }

		public bool IsPublished { get; set; }
	}

	public enum ContestType
	{
		SinglePlayerContest = 1,
		TeamContest = 2
	}
}
