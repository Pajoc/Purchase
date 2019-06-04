namespace Purchase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSupType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupplierType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Supplier", "TypeOfSupplierId", c => c.Int());
            CreateIndex("dbo.Supplier", "TypeOfSupplierId");
            AddForeignKey("dbo.Supplier", "TypeOfSupplierId", "dbo.SupplierType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Supplier", "TypeOfSupplierId", "dbo.SupplierType");
            DropIndex("dbo.Supplier", new[] { "TypeOfSupplierId" });
            DropColumn("dbo.Supplier", "TypeOfSupplierId");
            DropTable("dbo.SupplierType");
        }
    }
}
