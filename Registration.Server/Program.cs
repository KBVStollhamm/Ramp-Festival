using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			DatabaseSetup.Initialize();

			using (var processor = new RegistrationProcessor(false))
			{
				processor.Start();

				Console.WriteLine("Host started.");
				Console.WriteLine("Press ENTER to finish.");
				Console.ReadLine();

				processor.Stop();
			}
		}
	}
}
