using System;
using System.ComponentModel.DataAnnotations;

namespace BayesianETF.Models
{
    public class PreprocessedData
    {
        [Key]
        public int Id { get; set; }
        public string? Quarter { get; set; }
        public decimal GDP { get; set; }
        public decimal Inflation { get; set; }
        public decimal InterestRates { get; set; }
        public decimal Volatility { get; set; }
        public decimal MovingAvg { get; set; }
        public decimal Return { get; set; }
    }
}
