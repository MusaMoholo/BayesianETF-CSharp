import sqlite3
import pandas as pd
import os

# Paths
BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
db_path = os.path.join(BASE_DIR, "data", "stockdata.db")
conn = sqlite3.connect(db_path)

query = "SELECT * FROM ModelPredictions"
df_predictions = pd.read_sql(query, conn)

conn.close()