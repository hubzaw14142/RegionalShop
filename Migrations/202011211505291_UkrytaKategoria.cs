namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UkrytaKategoria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kategoria", "Ukryta", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Kategoria", "Ukryta");
        }
    }
}
