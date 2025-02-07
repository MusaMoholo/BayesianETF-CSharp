using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using BayesianETF.Models;

namespace BayesianETF.Services
{
    public class CsvService
    {
        public List<StockPrice> ReadStockData(string filePath)
        {
            var stockPrices = new List<StockPrice>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File '{filePath}' not found.");
                return stockPrices;
            }

            using (var reader = new StreamReader(filePath))
            {
                string? headerLine = reader.ReadLine(); // Read and ignore header
                if (headerLine == null) return stockPrices; // Check if file is empty

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine(); // Read a line from the CSV
                    if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

                    string[] values = line.Split(','); // Split the CSV line into an array
                    if (values.Length < 7) continue; // Ensure correct number of columns

                    try
                    {
                        var stockPrice = new StockPrice
                        {
                            Symbol = values[0] ?? "UNKNOWN", // Default if null
                            Date = DateTime.TryParseExact(values[1], "yyyy-MM-dd", 
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate) ? parsedDate : DateTime.MinValue,
                            Open = decimal.TryParse(values[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal open) ? open : 0,
                            High = decimal.TryParse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal high) ? high : 0,
                            Low = decimal.TryParse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal low) ? low : 0,
                            Close = decimal.TryParse(values[5], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal close) ? close : 0,
                            Volume = int.TryParse(values[6], NumberStyles.Any, CultureInfo.InvariantCulture, out int volume) ? volume : 0
                        };

                        stockPrices.Add(stockPrice);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing CSV line: {line}. Exception: {ex.Message}");
                    }
                }
            }

            return stockPrices;
        }

        public void SaveToDatabase(List<StockPrice> stockPrices)
        {
            using (var context = new DatabaseContext())
            {
                var quarterlyData = stockPrices
                    .GroupBy(stock => new { stock.Symbol, Quarter = $"{stock.Date.Year}-Q{(stock.Date.Month - 1) / 3 + 1}" })
                    .Select(g => new StockPrice
                    {
                        Symbol = g.Key.Symbol,
                        Quarter = g.Key.Quarter,
                        Open = g.OrderBy(s => s.Date).First().Open,
                        High = g.Max(s => s.High),
                        Low = g.Min(s => s.Low),
                        Close = g.OrderByDescending(s => s.Date).First().Close,
                        Volume = g.Sum(s => s.Volume)
                    })
                    .ToList();

                context.StockPrices.AddRange(quarterlyData);
                context.SaveChanges();

                Console.WriteLine($"Inserted {quarterlyData.Count} quarterly records into the database.");
            }
        }

    }
}
