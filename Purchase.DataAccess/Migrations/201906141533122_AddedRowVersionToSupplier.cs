namespace Purchase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRowVersionToSupplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Supplier", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Supplier", "RowVersion");
        }
    }
}
