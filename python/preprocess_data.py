import sqlite3
import pandas as pd
import os


BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
db_path = os.path.join(BASE_DIR, "data", "stockdata.db")
conn = sqlite3.connect(db_path)


query = "SELECT * FROM StockPrices"
df_stocks = pd.read_sql(query, conn)

query_macro = "SELECT * FROM MacroData"
df_macro = pd.read_sql(query_macro, conn)

df_stocks["Quarter"] = df_stocks["Quarter"].str.replace("Q", "-Q")
df_macro["Quarter"] = df_macro["Quarter"].str.replace("Q", "-Q")
df_stocks["Return"] = df_stocks.groupby("Symbol")["Close"].pct_change()  # Quarterly return
df_stocks["Volatility"] = df_stocks.groupby("Symbol")["Return"].rolling(4, min_periods=1).std().reset_index(0, drop=True)  # 4-quarter rolling volatility
df_stocks["MovingAvg"] = df_stocks.groupby("Symbol")["Close"].rolling(4, min_periods=1).mean().reset_index(0, drop=True)  # 4-quarter moving average

df_merged = df_stocks.merge(df_macro, on="Quarter", how="left")
df_merged.dropna(inplace=True)

df_merged.to_sql("PreprocessedData", conn, if_exists="replace", index=False)


conn.close()