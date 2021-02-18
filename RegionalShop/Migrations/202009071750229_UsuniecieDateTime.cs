namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuniecieDateTime : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Produkt", "DataDodania");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Produkt", "DataDodania", c => c.DateTime(nullable: false));
        }
    }
}
