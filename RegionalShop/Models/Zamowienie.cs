using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegionalShop.Models
{
    public class Zamowienie
    {
        public int ZamowienieId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Wprowadz imie")]
        [StringLength(25)]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Wprowadz nazwisko")]
        [StringLength(25)]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Wprowadz miejscowosc")]
        [StringLength(35)]
        public string Miejscowosc { get; set; }

        [Required(ErrorMessage = "Wprowadz Adres")]
        [StringLength(30)]
        public string Adres { get; set; }

        [Required(ErrorMessage = "Wprowadz kod pocztowy")]
        [StringLength(6)]
        public string KodPocztowy { get; set; }

        [Required(ErrorMessage = "Wprowadz numer telefonu")]
        [StringLength(20)]
        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Błędny format numeru telefonu.")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Wprowadz email")]
        [EmailAddress(ErrorMessage = "Błędny format adresu e-mail.")]
        public string Email { get; set; }

        [StringLength(300)]
        public string Komentarz { get; set; }

        public DateTime DataDodania { get; set; }
        public StanZamowienia StanZamowienia { get; set; }
        public decimal WartoscZamowienia { get; set; }
        public List<PozycjaZamowienia> PozycjeZamowienia { get; set; }

    }

    public enum StanZamowienia
    {
        Nowe,
        Zrealizowane
    }
}