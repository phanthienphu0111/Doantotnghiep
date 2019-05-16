namespace BS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFoundedDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrdersBooks", "FoundedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrdersBooks", "FoundedDate", c => c.DateTime(storeType: "date"));
        }
    }
}
