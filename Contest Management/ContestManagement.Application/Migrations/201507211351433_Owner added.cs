namespace ContestManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Owneradded : DbMigration
    {
        public override void Up()
        {
            AddColumn("ContestManagement.Contests", "OwnerName", c => c.String());
            AddColumn("ContestManagement.Contests", "OwnerEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("ContestManagement.Contests", "OwnerEmail");
            DropColumn("ContestManagement.Contests", "OwnerName");
        }
    }
}
