using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContestManagement.DataAccess;

namespace ContestManagement
{
	public class ContestService
	{
		public void CreateContest(ContestInfo contest)
		{
			using (var context = new ContestContext())
			{
				var existingSlug = context.Contests
					.Where(c => c.Slug == contest.Slug)
					.Select(c => c.Slug)
					.Any();

				if (existingSlug)
					throw new DuplicateNameException("The chosen contest slug is already taken.");

				// Contest publishing is explicit. 
				if (contest.IsPublished)
					contest.IsPublished = false;

				context.Contests.Add(contest);

				context.SaveChanges();

				//this.PublishConferenceEvent<ConferenceCreated>(conference);
			}
		}

		public ContestInfo FindContest(string slug)
		{
			using (var context = new ContestContext())
			{
				return context.Contests.FirstOrDefault(x => x.Slug == slug);
			}
		}

		public ContestInfo FindContest(string email, string accessCode)
		{
			using (var context = new ContestContext())
			{
				return context.Contests.FirstOrDefault(x => x.OwnerEmail == email && x.AccessCode == accessCode);
			}
		}
	}
}
