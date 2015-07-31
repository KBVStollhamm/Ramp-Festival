using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Messaging.Azure.Instrumentation;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Infrastructure.Messaging.Azure
{
	/// <summary>
	/// Implements an asynchronous receiver of messages from a Windows Azure Service Bus topic subscription.
	/// </summary>
	public class SubscriptionReceiver : IMessageReceiver, IDisposable
	{
		public void Start(Func<BrokeredMessage, MessageReleaseAction> messageHandler)
		{
			throw new NotImplementedException();
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}

		#region IDisposable

		/// <summary>
		/// Stops the listener if it was started previously.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			this.Stop();

			if (disposing)
			{
			}
		}

		#endregion
	}
}
