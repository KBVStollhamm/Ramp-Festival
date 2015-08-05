using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
	public class Sequencing
	{
		private Sequencing()
		{
			this.Sequence = new List<SequencingItem>();
		}

		public Sequencing(Guid contestId)
			: this()
		{
			this.ContestId = contestId;
		}

		public Sequencing(Guid contestId, IEnumerable<SequencingItem> sequence)
		{
			this.ContestId = contestId;
			this.Sequence = sequence.ToList();
		}

		public Guid ContestId { get; set; }
		public IList<SequencingItem> Sequence { get; set; }

		public int Version { get; set; }
	}
}
