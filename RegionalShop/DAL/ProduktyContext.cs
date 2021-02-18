using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using RegionalShop.Models;

namespace RegionalShop.DAL
{
    public class ProduktyContext : IdentityDbContext<ApplicationUser>
    {
        public ProduktyContext() : base("ProduktyContext")
        {

        }

        static ProduktyContext()
        {
            Database.SetInitializer<ProduktyContext>(new ProduktyInitializer());
        }

        public static ProduktyContext Create()
        {
            return new ProduktyContext();
        }

        public virtual DbSet<Produkt> Produkty { get; set; }
        public virtual DbSet<Kategoria> Kategorie { get; set; }
        public virtual DbSet<Zamowienie> Zamowienia { get; set; }
        public virtual DbSet<PozycjaZamowienia> PozycjeZamowienia { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // using System.Data.Entity.ModelConfiguration.Conventions;
            // Wyłącza konwencję, która automatycznie tworzy liczbę mnogą dla nazw tabel w bazie danych
            // Zamiast Kategorie zostałaby stworzona tabela o nazwie Kategories
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}