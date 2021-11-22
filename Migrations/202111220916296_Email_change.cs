namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Email_change : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "DaneUzytkownika_Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DaneUzytkownika_Email", c => c.String());
        }
    }
}
