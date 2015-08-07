using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EventSourcing.Sql;
using Registration.ReadModel.Implementation;
using Registration.ReadModel.Migrations;

namespace Registration.Server
{
	/// <summary>
	/// Initializes the EF infrastructure.
	/// </summary>
	internal static class DatabaseSetup
	{
		public static void Initialize()
		{
			Database.SetInitializer<EventStoreDbContext>(new CreateDatabaseIfNotExists<EventStoreDbContext>());
			Database.SetInitializer<RegistrationDbContext>(new MigrateDatabaseToLatestVersion<RegistrationDbContext, Configuration>());
		}
	}
}
