using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.ReadModel.Implementation
{
	public class ContestDbContext : DbContext
	{
		public const string SchemaName = "Registration";

		public ContestDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Sequencing>().ToTable("SequencingView", SchemaName);
			modelBuilder.Entity<Sequencing>().HasKey(e => e.ContestId);
			modelBuilder.Entity<Sequencing>().HasMany(e => e.Sequence).WithRequired();
			modelBuilder.Entity<SequencingItem>().ToTable("SequencingItemsView", SchemaName);
			modelBuilder.Entity<SequencingItem>().HasKey(e => new { e.GameId, e.Position });

			modelBuilder.Entity<Contest>().ToTable("ContestsView", SchemaName);
		}

		public T Find<T>(Guid id) where T : class
		{
			return this.Set<T>().Find(id);
		}

		public IQueryable<T> Query<T>() where T : class
		{
			return this.Set<T>();
		}

		public void Save<T>(T entity) where T : class
		{
			var entry = this.Entry(entity);

			if (entry.State == System.Data.Entity.EntityState.Detached)
				this.Set<T>().Add(entity);

			this.SaveChanges();
		}

		public IQueryable<Sequencing> Sequencing
		{
			get { return this.Set<Sequencing>(); }
		}
	}
}
