namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EdycjaKlasyZamowienieDodanieRelacji : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Zamowienie", name: "ApplicationUser_Id", newName: "UserId");
            RenameIndex(table: "dbo.Zamowienie", name: "IX_ApplicationUser_Id", newName: "IX_UserId");
            AlterColumn("dbo.Zamowienie", "Adres", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Zamowienie", "Telefon", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Zamowienie", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Zamowienie", "Email", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Zamowienie", "Telefon", c => c.String());
            AlterColumn("dbo.Zamowienie", "Adres", c => c.String(maxLength: 30));
            RenameIndex(table: "dbo.Zamowienie", name: "IX_UserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Zamowienie", name: "UserId", newName: "ApplicationUser_Id");
        }
    }
}
