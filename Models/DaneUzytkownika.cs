using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegionalShop.Models
{
    public class DaneUzytkownika
    {
        public string Imie { get; set; }

        public string Nazwisko { get; set; }

        public string Adres { get; set; }

        public string Miejscowosc { get; set; }

        public string KodPocztowy { get; set; }

        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Błędny format numeru telefonu.")]
        public string Telefon { get; set; }
    }
}