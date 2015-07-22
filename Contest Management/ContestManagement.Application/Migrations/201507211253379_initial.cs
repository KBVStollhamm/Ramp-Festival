namespace ContestManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ContestManagement.Contests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccessCode = c.String(),
                        Slug = c.String(),
                        WasEverPublished = c.Boolean(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Location = c.String(),
                        Tagline = c.String(),
                        TwitterSearch = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("ContestManagement.Contests");
        }
    }
}
