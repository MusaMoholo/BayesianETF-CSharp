import sqlite3
from sklearn.preprocessing import StandardScaler
import pandas as pd
import pymc as pm
import numpy as np
import os
import arviz as az



BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
db_path = os.path.join(BASE_DIR, "data", "stockdata.db")
conn = sqlite3.connect(db_path)

query = "SELECT * FROM PreprocessedData"
df = pd.read_sql(query, conn)

X = df[["GDP", "Inflation", "InterestRates", "Volatility", "MovingAvg"]].values
y = df["Return"].values

scaler = StandardScaler()
X_standardized = scaler.fit_transform(X)

# Define Bayesian Neural Network with PyMC
with pm.Model() as model:
    weights = pm.Normal("weights", mu=0, sigma=1, shape=X.shape[1])
    bias = pm.Normal("bias", mu=0, sigma=1)
    mu = pm.math.dot(X_standardized, weights) + bias
    sigma = pm.HalfNormal("sigma", sigma=1)
    
    returns = pm.Normal("returns", mu=mu, sigma=sigma, observed=y)
    trace = pm.sample(1000, return_inferencedata=True)

posterior_samples = trace.posterior

weights_mean = posterior_samples["weights"].mean(dim=["chain", "draw"]).values
bias_mean = posterior_samples["bias"].mean(dim=["chain", "draw"]).values
predicted_std = posterior_samples["sigma"].mean(dim=["chain", "draw"]).values

predicted_means = np.dot(X_standardized, weights_mean) + bias_mean
predicted_variances = np.full(df.shape[0], predicted_std**2)

df["PredictedReturn"] = predicted_means.flatten()
df["PredictedVariance"] = predicted_variances.flatten()
df.to_sql("ModelPredictions", conn, if_exists="replace", index=False)

conn.close()
