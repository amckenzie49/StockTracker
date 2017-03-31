using System;
using System.Collections.Generic;

namespace StockTracker.Entities
{
    public class StockClose
    {
        public decimal CloseValue { get; }
        public decimal AdjustedClose { get; }
        public DateTime CloseDate { get; }

        public StockClose(decimal close, DateTime date,decimal adjclose)
        {
            CloseValue = close;
            CloseDate = date;
            AdjustedClose = adjclose;
        }
    }
    public class Stock
    {
        public string TickerSymbol { get; set; }
        public string Name { get; set; }
        public decimal Ask { get; set; }
        public List<StockClose> AnnualClosingValues { get; set;}

        public Stock()
        {
            AnnualClosingValues = new List<Entities.StockClose>();
        }

    }
}
