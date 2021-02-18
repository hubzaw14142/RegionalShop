namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kategoria",
                c => new
                    {
                        KategoriaId = c.Int(nullable: false, identity: true),
                        NazwaKategorii = c.String(nullable: false, maxLength: 50),
                        OpisKategorii = c.String(nullable: false, maxLength: 100),
                        NazwaPlikuIkony = c.String(),
                    })
                .PrimaryKey(t => t.KategoriaId);
            
            CreateTable(
                "dbo.Produkt",
                c => new
                    {
                        ProduktId = c.Int(nullable: false, identity: true),
                        KategoriaId = c.Int(nullable: false),
                        NazwaProduktu = c.String(nullable: false, maxLength: 50),
                        NazwaPlikuObrazka = c.String(maxLength: 100),
                        OpisProduktu = c.String(nullable: false, maxLength: 300),
                        CenaProduktu = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DostepnyProdukt = c.Boolean(nullable: false),
                        OpisSkrocony = c.String(),
                        DataDodania = c.DateTime(nullable: false),
                        Bestseller = c.Boolean(nullable: false),
                        Ukryty = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProduktId)
                .ForeignKey("dbo.Kategoria", t => t.KategoriaId, cascadeDelete: true)
                .Index(t => t.KategoriaId);
            
            CreateTable(
                "dbo.PozycjaZamowienia",
                c => new
                    {
                        PozycjaZamowieniaId = c.Int(nullable: false, identity: true),
                        ZamowienieId = c.Int(nullable: false),
                        ProduktId = c.Int(nullable: false),
                        Ilosc = c.Int(nullable: false),
                        CenaZakupu = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PozycjaZamowieniaId)
                .ForeignKey("dbo.Produkt", t => t.ProduktId, cascadeDelete: true)
                .ForeignKey("dbo.Zamowienie", t => t.ZamowienieId, cascadeDelete: true)
                .Index(t => t.ZamowienieId)
                .Index(t => t.ProduktId);
            
            CreateTable(
                "dbo.Zamowienie",
                c => new
                    {
                        ZamowienieId = c.Int(nullable: false, identity: true),
                        Imie = c.String(nullable: false, maxLength: 25),
                        Nazwisko = c.String(nullable: false, maxLength: 25),
                        Ulica = c.String(maxLength: 35),
                        Miejscowosc = c.String(nullable: false, maxLength: 35),
                        NrLokalu = c.String(maxLength: 10),
                        KodPocztowy = c.String(nullable: false, maxLength: 6),
                        Telefon = c.String(),
                        Email = c.String(nullable: false, maxLength: 35),
                        Komentarz = c.String(maxLength: 300),
                        DataDodania = c.DateTime(nullable: false),
                        StanZamowienia = c.Int(nullable: false),
                        WartoscZamowienia = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ZamowienieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PozycjaZamowienia", "ZamowienieId", "dbo.Zamowienie");
            DropForeignKey("dbo.PozycjaZamowienia", "ProduktId", "dbo.Produkt");
            DropForeignKey("dbo.Produkt", "KategoriaId", "dbo.Kategoria");
            DropIndex("dbo.PozycjaZamowienia", new[] { "ProduktId" });
            DropIndex("dbo.PozycjaZamowienia", new[] { "ZamowienieId" });
            DropIndex("dbo.Produkt", new[] { "KategoriaId" });
            DropTable("dbo.Zamowienie");
            DropTable("dbo.PozycjaZamowienia");
            DropTable("dbo.Produkt");
            DropTable("dbo.Kategoria");
        }
    }
}
