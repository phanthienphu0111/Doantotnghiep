namespace BS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        PenName = c.String(maxLength: 100),
                        Overview = c.String(),
                        ImageBoolID = c.Int(),
                    })
                .PrimaryKey(t => t.AuthorID)
                .ForeignKey("dbo.ImageBool", t => t.ImageBoolID)
                .Index(t => t.ImageBoolID);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PublisherID = c.Int(),
                        PublicationDate = c.DateTime(storeType: "date"),
                        ImageBoolID = c.Int(),
                        Overview = c.String(),
                        Details = c.String(),
                        Price = c.Double(),
                        TotalQuantity = c.Int(nullable: false),
                        ViewCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.ImageBool", t => t.ImageBoolID)
                .ForeignKey("dbo.Publishers", t => t.PublisherID)
                .Index(t => t.PublisherID)
                .Index(t => t.ImageBoolID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Content = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        isDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Birthday = c.DateTime(nullable: false, storeType: "date"),
                        Phone = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        ImageURL = c.String(),
                        UserRoleID = c.Int(nullable: false),
                        isActivated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.UserRoles", t => t.UserRoleID)
                .Index(t => t.UserRoleID);
            
            CreateTable(
                "dbo.OrdersBooks",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        FoundedDate = c.DateTime(storeType: "date"),
                        UserID = c.Int(nullable: false),
                        Status = c.Int(),
                        Address = c.String(),
                        Phone = c.String(maxLength: 50, fixedLength: true),
                        Email = c.String(maxLength: 50, fixedLength: true),
                        FullName = c.String(),
                        Paid = c.Boolean(),
                        Canceled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.OrdersDetails",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                        Total = c.Double(),
                        Quantity = c.Int(),
                        CreatedDate = c.DateTime(storeType: "date"),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => new { t.OrderID, t.BookID })
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.OrdersBooks", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.BookID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserRoleID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50, fixedLength: true),
                    })
                .PrimaryKey(t => t.UserRoleID);
            
            CreateTable(
                "dbo.ImageBool",
                c => new
                    {
                        ImageBoolID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ImageBoolID);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        ImageBoolID = c.Int(nullable: false),
                        ImageURL = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ImageID)
                .ForeignKey("dbo.ImageBool", t => t.ImageBoolID)
                .Index(t => t.ImageBoolID);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        PublisherID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(),
                        Email = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 20),
                        Note = c.String(),
                        ImageURL = c.String(),
                    })
                .PrimaryKey(t => t.PublisherID);
           
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Phone = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.FilterPrice",
                c => new
                    {
                        FilterID = c.Int(nullable: false),
                        PriceFrom = c.Double(),
                        PriceTo = c.Double(),
                    })
                .PrimaryKey(t => t.FilterID);
            
            CreateTable(
                "dbo.historyBankChargings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        fullname = c.String(),
                        phone = c.String(),
                        email = c.String(),
                        transaction_info = c.String(),
                        order_code = c.String(),
                        price = c.Int(),
                        payment_id = c.String(),
                        payment_type = c.String(),
                        error_text = c.String(),
                        secure_code = c.String(),
                        date_trans = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        PayedID = c.Int(nullable: false, identity: true),
                        PayName = c.String(maxLength: 10, fixedLength: true),
                        Note = c.String(),
                        OrderID = c.Int(),
                    })
                .PrimaryKey(t => t.PayedID);
            
            CreateTable(
                "dbo.CategoriesBooks",
                c => new
                    {
                        BookID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookID, t.CategoryID })
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.BookID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.AuthorsBooks",
                c => new
                    {
                        AuthorID = c.Int(nullable: false),
                        BookID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuthorID, t.BookID })
                .ForeignKey("dbo.Authors", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.BookID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorsBooks", "BookID", "dbo.Books");
            DropForeignKey("dbo.AuthorsBooks", "AuthorID", "dbo.Authors");
            DropForeignKey("dbo.Books", "PublisherID", "dbo.Publishers");
            DropForeignKey("dbo.Images", "ImageBoolID", "dbo.ImageBool");
            DropForeignKey("dbo.Books", "ImageBoolID", "dbo.ImageBool");
            DropForeignKey("dbo.Authors", "ImageBoolID", "dbo.ImageBool");
            DropForeignKey("dbo.Users", "UserRoleID", "dbo.UserRoles");
            DropForeignKey("dbo.OrdersBooks", "UserID", "dbo.Users");
            DropForeignKey("dbo.OrdersDetails", "OrderID", "dbo.OrdersBooks");
            DropForeignKey("dbo.OrdersDetails", "BookID", "dbo.Books");
            DropForeignKey("dbo.Comments", "UserID", "dbo.Users");
            DropForeignKey("dbo.Comments", "BookID", "dbo.Books");
            DropForeignKey("dbo.CategoriesBooks", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.CategoriesBooks", "BookID", "dbo.Books");
            DropIndex("dbo.AuthorsBooks", new[] { "BookID" });
            DropIndex("dbo.AuthorsBooks", new[] { "AuthorID" });
            DropIndex("dbo.CategoriesBooks", new[] { "CategoryID" });
            DropIndex("dbo.CategoriesBooks", new[] { "BookID" });
            DropIndex("dbo.Images", new[] { "ImageBoolID" });
            DropIndex("dbo.OrdersDetails", new[] { "BookID" });
            DropIndex("dbo.OrdersDetails", new[] { "OrderID" });
            DropIndex("dbo.OrdersBooks", new[] { "UserID" });
            DropIndex("dbo.Users", new[] { "UserRoleID" });
            DropIndex("dbo.Comments", new[] { "BookID" });
            DropIndex("dbo.Comments", new[] { "UserID" });
            DropIndex("dbo.Books", new[] { "ImageBoolID" });
            DropIndex("dbo.Books", new[] { "PublisherID" });
            DropIndex("dbo.Authors", new[] { "ImageBoolID" });
            DropTable("dbo.AuthorsBooks");
            DropTable("dbo.CategoriesBooks");
            DropTable("dbo.Payment");
            DropTable("dbo.historyBankChargings");
            DropTable("dbo.FilterPrice");
            DropTable("dbo.Customers");
            DropTable("dbo.Publishers");
            DropTable("dbo.Images");
            DropTable("dbo.ImageBool");
            DropTable("dbo.UserRoles");
            DropTable("dbo.OrdersDetails");
            DropTable("dbo.OrdersBooks");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
