import sqlite3
import pandas as pd
import os

BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
db_path = os.path.join(BASE_DIR, "data", "stockdata.db")
conn = sqlite3.connect(db_path)

query = "SELECT Quarter, PredictedReturn FROM ModelPredictions"
df_predictions = pd.read_sql(query, conn)

df_aggregated = df_predictions.groupby("Quarter").mean().reset_index()
df_aggregated.to_sql("QuarterlyPredictions", conn, if_exists="replace", index=False)

conn.close()
