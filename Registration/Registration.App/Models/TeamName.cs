using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Models
{
	public class TeamName : IEquatable<TeamName>
	{
		public static TeamName None = new TeamName(string.Empty);

		public TeamName(string name)
		{
			this.Value = name;
		}

		public string Value { get; private set; }

		public bool Equals(TeamName other)
		{
			if (other == null) return false;
			return (this.Value.Equals(other.Value));
		}

		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			TeamName t = obj as TeamName;
			if (t != null)
			{
				return Equals(t);
			}
			else
			{
				return false;
			}
		}
	}
}
