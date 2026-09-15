using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseManager.Models
{

    public enum ExpenseCategory
{
        Jedzenie,
        Dom,
        Rozrywka,
}
    public class Expense
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany.")]
        [StringLength(100, ErrorMessage = "Opis nie może przekraczać 100 znaków.")]
        [Display(Name = "Opis wydatku")]
        public string Description { get; set; } = string.Empty;

        private decimal? _price;

        [Required(ErrorMessage = "Kwota jest wymagana.")]
        [Range(0.01, 1000000.00, ErrorMessage = "Kwota musi być większa od 0.")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Kwota")]
        public decimal? Price
        {
            get => _price;
            set => _price = value;
        }

        [Required(ErrorMessage = "Wybierz kategorię.")]
        [Display(Name = "Kategoria")]
        public ExpenseCategory Category { get; set; }

        [Required(ErrorMessage = "Data jest wymagana.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
