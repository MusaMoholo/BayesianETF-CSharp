using System;
using System.ComponentModel.DataAnnotations;

namespace BayesianETF.Models
{
    public class StockPrice
    {
        [Key]
        public int Id { get; set; }

        public string? Symbol { get; set; }  // Stock ticker
        public string? Quarter { get; set; } // "YYYY-QX" (e.g., "2024-Q1")
        public decimal Open { get; set; }   // First Open price of the quarter
        public decimal High { get; set; }   // Max High in the quarter
        public decimal Low { get; set; }    // Min Low in the quarter
        public decimal Close { get; set; }  // Last Close price of the quarter
        public long Volume { get; set; }    // Sum of trading volume in the quarter
    }
}
