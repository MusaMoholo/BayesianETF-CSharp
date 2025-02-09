using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BayesianETF.Models;

namespace BayesianETF.Services
{
    public class QueryService
    {
        private readonly DbContextOptions<DatabaseContext> _options;

        public QueryService(string databasePath)
        {
            Console.WriteLine($"Database path: {databasePath}");

            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite($"Data Source={databasePath}")
                .Options;
        }

        public List<ModelPrediction> FetchModelPredictions()
        {
            using var context = new DatabaseContext(_options);

            Console.WriteLine("Fetching model predictions from the database...");
            List<ModelPrediction> predictions = context.ModelPredictions.ToList();

            foreach (var prediction in predictions)
            {
                Console.WriteLine($"{prediction.Symbol}, {prediction.Quarter}: Predicted Return = {prediction.PredictedReturn}, Variance = {prediction.PredictedVariance}");
            }

            return predictions;
        }
    }
}
