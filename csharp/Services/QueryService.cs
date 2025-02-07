using System;
using System.Collections.Generic;
using System.Linq;
using BayesianETF.Models;

namespace BayesianETF.Services
{
    public class QueryService
    {
        public void FetchAllStocks()
        {
            using (var context = new DatabaseContext())
            {
                var stocks = context.StockPrices.ToList();
                foreach (var stock in stocks)
                {
                    Console.WriteLine($"{stock.Date.ToShortDateString()} {stock.Symbol}: Open {stock.Open}, Close {stock.Close}");
                }
            }
        }

        public void GetTop5QuarterlyStocksByReturn()
        {
            using (var context = new DatabaseContext())
            {
                var topStocks = context.StockPrices
                    .AsEnumerable() // Needed to avoid SQLite decimal ordering issues
                    .OrderByDescending(stock => (double)((stock.Close - stock.Open) / stock.Open))
                    .Take(5)
                    .ToList();

                Console.WriteLine("\nTop 5 Stocks by Quarterly Return:");
                foreach (var stock in topStocks)
                {
                    double quarterlyReturn = (double)((stock.Close - stock.Open) / stock.Open) * 100;
                    Console.WriteLine($"{stock.Symbol} ({stock.Quarter}): {quarterlyReturn:F2}% return (Open: {stock.Open}, Close: {stock.Close})");
                }
            }
        }

    }
}
