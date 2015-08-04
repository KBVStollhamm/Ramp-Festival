using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Models
{
	public class PlayerName
	{
		public PlayerName(string fullName)
		{
			this.FullName = fullName;
		}

		public string FullName { get; private set; }
	}
}
