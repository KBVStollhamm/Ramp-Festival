using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Azure.Instrumentation
{
	public interface ISubscriptionReceiverInstrumentation
	{
		void MessageReceived();

		void MessageProcessed(bool success, long elapsedMilliseconds);

		void MessageCompleted(bool success);
	}
}
