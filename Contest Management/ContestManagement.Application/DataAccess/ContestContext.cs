using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContestManagement.DataAccess
{
	public class ContestContext : DbContext
	{
		public const string SchemaName = "ContestManagement";

		public ContestContext()
			: this("ContestManagement")
		{
		}

		public ContestContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		public virtual DbSet<ContestInfo> Contests { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ContestInfo>().ToTable("Contests", SchemaName);
		}
	}
}
