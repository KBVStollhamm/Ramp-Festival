using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Messaging.Handling;
using Infrastructure.Serialization;

namespace Infrastructure.Messaging.Azure
{
	/// <summary>
	/// Processes incoming commands from the bus and routes them to the appropriate 
	/// handlers.
	/// </summary>
	public class CommandProcessor : MessageProcessor, ICommandHandlerRegistry
	{
		private readonly CommandDispatcher commandDispatcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandProcessor"/> class.
		/// </summary>
		/// <param name="receiver">The receiver to use. If the receiver is <see cref="IDisposable"/>, it will be disposed when the processor is 
		/// disposed.</param>
		/// <param name="serializer">The serializer to use for the message body.</param>
		public CommandProcessor(IMessageReceiver receiver, ITextSerializer serializer)
			: base(receiver, serializer)
		{
			this.commandDispatcher = new CommandDispatcher();
		}

		/// <summary>
		/// Registers the specified command handler.
		/// </summary>
		public void Register(ICommandHandler commandHandler)
		{
			this.commandDispatcher.Register(commandHandler);
		}

		/// <summary>
		/// Processes the message by calling the registered handler.
		/// </summary>
		protected override void ProcessMessage(string traceIdentifier, object payload, string messageId, string correlationId)
		{
			this.commandDispatcher.ProcessMessage(traceIdentifier, (ICommand)payload, messageId, correlationId);
		}
	}
}
