using System;
using BayesianETF.Services;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "Data/stock_prices.csv";
        var csvService = new CsvService();
        var stockData = csvService.ReadStockData(filePath);

        // Insert data into the database if it's not empty
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

        // Fetch and display all stock prices
        Console.WriteLine("\nFetching all stock prices:");
        queryService.FetchAllStocks();

        // Fetch and display top 5 stocks by return
        Console.WriteLine("\nFetching top 5 stocks by return:");
        queryService.GetTop5StocksByReturn();
    }
}
