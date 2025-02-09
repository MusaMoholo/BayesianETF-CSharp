using BayesianETF.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BayesianETF.Services
{
    public class ETFService
    {
        private readonly string _databasePath;

        public ETFService(string databasePath)
        {
            _databasePath = databasePath;
        }

        public List<ETFComposition> BuildETF(double maxWeight = 0.15)
        {
            using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT Symbol, PredictedVariance FROM ModelPredictions";
                var reader = command.ExecuteReader();

                var predictions = new List<(string Symbol, double PredictedVariance)>();

                while (reader.Read())
                {
                    predictions.Add((reader.GetString(0), reader.GetDouble(1)));
                }

                connection.Close();

                // Calculate inverse variance and normalize weights
                var inverseVariance = predictions.Select(p => 1 / p.PredictedVariance).ToList();
                var totalInverseVariance = inverseVariance.Sum();

                var etf = predictions.Select((p, i) =>
                {
                    var weight = inverseVariance[i] / totalInverseVariance;
                    return new ETFComposition { Symbol = p.Symbol, Weight = Math.Min(weight, maxWeight) };
                }).ToList();

                // Renormalize weights
                var totalWeight = etf.Sum(e => e.Weight);
                etf.ForEach(e => e.Weight /= totalWeight);

                // Save to the database
                SaveETFToDatabase(etf);

                return etf;
            }
        }

        private void SaveETFToDatabase(List<ETFComposition> etf)
        {
            using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM ETFComposition";
                command.ExecuteNonQuery();

                foreach (var entry in etf)
                {
                    command.CommandText = $"INSERT INTO ETFComposition (Symbol, Weight) VALUES ('{entry.Symbol}', {entry.Weight})";
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
