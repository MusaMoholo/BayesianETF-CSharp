import yfinance as yf
import pandas as pd
import sqlite3
import os


tickers = ["AAPL", "MSFT", "GOOGL", "AMZN", "TSLA"]


BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
db_path = os.path.join(BASE_DIR, "data", "stockdata.db")
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

cursor.execute("""
CREATE TABLE IF NOT EXISTS StockPrices (
    Symbol TEXT,
    Quarter TEXT,
    Open REAL,
    High REAL,
    Low REAL,
    Close REAL,
    Volume INTEGER
)
""")

def fetch_quarterly_stock_data(ticker):
    stock = yf.Ticker(ticker)

    hist = stock.history(period="10y", interval="1d")
    hist.index = hist.index.tz_localize(None)
    hist["Quarter"] = hist.index.to_period("Q").astype(str)

    quarterly_data = hist.groupby("Quarter").agg(
        Open=("Open", "first"),
        High=("High", "max"),
        Low=("Low", "min"),
        Close=("Close", "last"),
        Volume=("Volume", "sum")
    ).reset_index()

    quarterly_data["Symbol"] = ticker

    quarterly_data.to_sql("StockPrices", conn, if_exists="append", index=False)

for ticker in tickers:
    fetch_quarterly_stock_data(ticker)

macro_data = pd.DataFrame({
    "Quarter": ["2022Q1", "2022Q2", "2022Q3", "2022Q4"],
    "GDP": [1.5, 2.1, 1.8, 1.9],
    "Inflation": [2.3, 2.8, 3.0, 3.2],
    "InterestRates": [0.5, 0.75, 1.0, 1.25]
})

macro_data.to_sql("MacroData", conn, if_exists="replace", index=False)

conn.close()

