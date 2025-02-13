This project is a comprehensive full-stack pipeline that predicts quarterly stock returns and variances, then constructs a personalized exchange-traded fund (ETF) using Bayesian-weighted optimization. 
Combining machine learning, Python, C#, and SQL, the framework offers a robust approach to portfolio construction and optimization. 
The predictive component is driven by a Bayesian Neural Network (BNN), which forecasts stock returns and variance over a financial quarter using macroeconomic and market data. 
Predictions are stored in a SQLite database, and the ETF is constructed using a C# application that queries the data and allocates weights to stocks based on their predicted return and associated uncertainty.

The pipeline is designed to maximize efficiency and accuracy. Data preprocessing and model training are performed in Python, where the BNN leverages PyMC3 to provide probabilistic predictions. 
The predictions are then saved to a SQL database. 
The C# application, powered by Entity Framework, queries these predictions and constructs the personalized ETF by weighting stocks according to their return-to-risk ratio. 
This approach ensures a balanced allocation that favors high-return, low-variance stocks.

By combining Python’s strengths in machine learning with C#’s robust data-handling and performance capabilities, this project demonstrates a real-world application of quantitative finance principles. 
The SQLite database serves as a lightweight and efficient storage layer, enabling seamless interaction between the predictive and portfolio construction stages.
