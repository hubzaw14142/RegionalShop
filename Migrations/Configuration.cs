namespace RegionalShop.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using RegionalShop.DAL;

    public sealed class Configuration : DbMigrationsConfiguration<RegionalShop.DAL.ProduktyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "RegionalShop.DAL.ProduktyContext";
        }

        protected override void Seed(RegionalShop.DAL.ProduktyContext context)
        {

            ProduktyInitializer.SeedProduktyData(context);
            ProduktyInitializer.SeedUzytkownicy(context);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
