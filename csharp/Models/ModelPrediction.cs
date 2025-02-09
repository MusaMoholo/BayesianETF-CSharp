namespace BayesianETF.Models
{
    public class ModelPrediction
    {
        public int Id { get; set; }
        public string? Quarter { get; set; }
        public string? Symbol { get; set; }
        public double PredictedReturn { get; set; }
        public double PredictedVariance { get; set; }
    }
}
