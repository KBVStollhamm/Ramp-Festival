using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Models
{
	public class Team
	{
		public Team(string name)
		{
			this.Name = new TeamName(name);
		}

		public TeamName Name { get; private set; }
	}
}
