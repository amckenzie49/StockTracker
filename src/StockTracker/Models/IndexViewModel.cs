using System.ComponentModel.DataAnnotations;
using StockTracker.Entities;

namespace StockTracker.Models
{
    public class IndexViewModel
    {
        [Display(Name = "Ticker Symbol"), Required]
        public string TickerSymbol { get; set; }

        public Stock CurrentStock { get; set; }

        //public decimal Ask { get; set; }
    }
}
