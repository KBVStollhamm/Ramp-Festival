using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContestManagement.Utils;

namespace ContestManagement
{
	/// <summary>
	/// Editable information about a contest.
	/// </summary>
	public class EditableContestInfo
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public string Location { get; set; }

		public string Tagline { get; set; }
		public string TwitterSearch { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public bool IsPublished { get; set; }
	}

	/// <summary>
	/// The full contest information.
	/// </summary>
	/// <remarks>
	/// This class inherites from <see cref="EditableContestInfo"/>
	/// and exposes more information that is not user-editable
	/// once is has been generated or provided.
	/// </remarks>
	public class ContestInfo : EditableContestInfo
	{
		public ContestInfo()
		{
			this.Id = GuidUtil.NewSequentialId();
			this.AccessCode = HandleGenerator.Generate(6);

		}

		public Guid Id { get; set; }

		public string AccessCode { get; set; }

		public string OwnerName { get; set; }

		public string OwnerEmail { get; set; }

		public string Slug { get; set; }

		public bool WasEverPublished { get; set; }
	}
}
