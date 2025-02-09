using System;
using BayesianETF.Services;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "Data/stock_prices.csv";
        var csvService = new CsvService();
        var stockData = csvService.ReadStockData(filePath);

        if (stockData.Count > 0)
        {
            csvService.SaveToDatabase(stockData);
            Console.WriteLine($"Inserted {stockData.Count} records into the database.");
        }
        else
        {
            Console.WriteLine("No stock data found in CSV.");
        }

        var queryService = new QueryService();

        Console.WriteLine("\nFetching all stock prices:");
        queryService.FetchAllStocks();

        Console.WriteLine("\nFetching top 5 stocks by return:");
        queryService.GetTop5StocksByReturn();
    }
}
