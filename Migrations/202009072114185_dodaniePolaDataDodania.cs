namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodaniePolaDataDodania : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produkt", "DataDodania", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produkt", "DataDodania");
        }
    }
}
