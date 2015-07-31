using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace Registration.Server
{
	public  class TestProcessor: IProcessor
	{
		public void Start()
		{
			Console.WriteLine("Starting TestProcessor...");
		}

		public void Stop()
		{
			Console.WriteLine("Stopping TestProcessor...");
		}
	}
}
