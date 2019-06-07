namespace Purchase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMeeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SupplierMeeting",
                c => new
                    {
                        Supplier_Id = c.Int(nullable: false),
                        Meeting_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Supplier_Id, t.Meeting_Id })
                .ForeignKey("dbo.Supplier", t => t.Supplier_Id, cascadeDelete: true)
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id, cascadeDelete: true)
                .Index(t => t.Supplier_Id)
                .Index(t => t.Meeting_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierMeeting", "Meeting_Id", "dbo.Meeting");
            DropForeignKey("dbo.SupplierMeeting", "Supplier_Id", "dbo.Supplier");
            DropIndex("dbo.SupplierMeeting", new[] { "Meeting_Id" });
            DropIndex("dbo.SupplierMeeting", new[] { "Supplier_Id" });
            DropTable("dbo.SupplierMeeting");
            DropTable("dbo.Meeting");
        }
    }
}
