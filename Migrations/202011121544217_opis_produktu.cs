namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class opis_produktu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Produkt", "OpisProduktu", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Produkt", "OpisProduktu", c => c.String(nullable: false, maxLength: 300));
        }
    }
}
