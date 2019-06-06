namespace Purchase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSupplierPhoneNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupplierPhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        SupplierId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Supplier", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.SupplierId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplierPhoneNumber", "SupplierId", "dbo.Supplier");
            DropIndex("dbo.SupplierPhoneNumber", new[] { "SupplierId" });
            DropTable("dbo.SupplierPhoneNumber");
        }
    }
}
