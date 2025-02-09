using System;
using System.Collections.Generic;
using BayesianETF.Models;
using BayesianETF.Services;

class Program
{
    static void Main(string[] args)
    {
        string databasePath = "/home/musamoholo98/BayesianETF/data/stockdata.db"; // Ensure this path is correct.
        var queryService = new QueryService(databasePath);

        try
        {
            List<ModelPrediction> predictions = queryService.FetchModelPredictions();

            Console.WriteLine("Fetched model predictions:");
            foreach (var prediction in predictions)
            {
                Console.WriteLine($"{prediction.Symbol}, {prediction.Quarter}: Predicted Return = {prediction.PredictedReturn}, Predicted Variance = {prediction.PredictedVariance}");
            }

            Console.WriteLine("ETF construction not implemented yet. Placeholder for future extension.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
