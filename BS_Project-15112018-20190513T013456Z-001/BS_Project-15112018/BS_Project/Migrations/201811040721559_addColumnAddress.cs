namespace BS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Address");
        }
    }
}
