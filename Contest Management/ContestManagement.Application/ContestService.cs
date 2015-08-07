using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContestManagement.DataAccess;
using ContestManagement.Events;
using MassTransit;

namespace ContestManagement
{
	public class ContestService
	{
		private readonly IServiceBus _eventBus;

		public ContestService(IServiceBus eventBus)
		{
			_eventBus = eventBus;
		}

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

				this.PublishContestEvent<ContestCreated>(contest);
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

		private void PublishContestEvent<T>(ContestInfo contest)
			where T : ContestEvent, new()
		{
			_eventBus.Publish(new T()
			{
				SourceId = contest.Id,
				Name = contest.Name,
				Description = contest.Description,
				Location = contest.Location,
				Slug = contest.Slug,
				Tagline = contest.Tagline,
				TwitterSearch = contest.TwitterSearch,
				StartDate = contest.StartDate,
				EndDate = contest.EndDate,
			});
		}
	}
}
