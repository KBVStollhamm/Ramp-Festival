using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging
{
	/// <summary>
	/// Represents a command message.
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// Gets the command identifier.
		/// </summary>
		Guid Id { get; }
	}
}
