namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmiany_kategoria : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Kategoria", "OpisKategorii", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Kategoria", "OpisKategorii", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
