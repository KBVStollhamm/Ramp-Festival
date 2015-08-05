using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing.Sql;

namespace Registration.Server
{
	/// <summary>
	/// Initializes the EF infrastructure.
	/// </summary>
	internal static class DatabaseSetup
	{
		public static void Initialize()
		{
			//Database.DefaultConnectionFactory = new ServiceConfigurationSettingConnectionFactory(Database.DefaultConnectionFactory);

			Database.SetInitializer<EventStoreDbContext>(new CreateDatabaseIfNotExists<EventStoreDbContext>());
		}
	}
}
