namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EdycjaKlasyZamowienie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Zamowienie", "Adres", c => c.String(maxLength: 30));
            AddColumn("dbo.AspNetUsers", "DaneUzytkownika_Miejscowosc", c => c.String());
            DropColumn("dbo.Zamowienie", "Ulica");
            DropColumn("dbo.Zamowienie", "NrLokalu");
            DropColumn("dbo.AspNetUsers", "DaneUzytkownika_Miasto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DaneUzytkownika_Miasto", c => c.String());
            AddColumn("dbo.Zamowienie", "NrLokalu", c => c.String(maxLength: 10));
            AddColumn("dbo.Zamowienie", "Ulica", c => c.String(maxLength: 35));
            DropColumn("dbo.AspNetUsers", "DaneUzytkownika_Miejscowosc");
            DropColumn("dbo.Zamowienie", "Adres");
        }
    }
}
