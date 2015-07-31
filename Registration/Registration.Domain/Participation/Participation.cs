using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing;

namespace Registration.Domain
{
	public class Participation : EventSourced
	{
		protected Participation(Guid id) : base(id)
		{
		}

		public Participation(Guid id, IEnumerable<IVersionedEvent> history)
            : this(id)
        {
			this.LoadFrom(history);
		}

		public Participation(Guid id, string playerName)
			: this(id)
		{
			//TODO: Implement logic
		}
	}
}
