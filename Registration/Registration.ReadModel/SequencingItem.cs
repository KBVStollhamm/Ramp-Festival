using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel
{
	public class SequencingItem
	{
		internal SequencingItem()
		{
		}

		public SequencingItem(int position)
		{
			this.SequencingId = Guid.NewGuid();
			this.Position = position;
		}

		public Guid SequencingId { get; set; }
		public int Position {get ;set;}
		public string PlayerName { get; set; }
		public string TeamName { get; set; }
	}
}
