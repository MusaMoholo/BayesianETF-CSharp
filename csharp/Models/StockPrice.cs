using System;
using System.ComponentModel.DataAnnotations;

namespace BayesianETF.Models
{
    public class StockPrice
    {
        [Key]
        public int Id { get; set; }
        public string? Symbol { get; set; }
        public string? Quarter { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
    }
}
