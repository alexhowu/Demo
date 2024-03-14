using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DemoApp.Models;
using Newtonsoft.Json;

namespace DemoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockDataController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<StockModel>> Get(string symbol)
        {

            var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey=7MDZMLASSO853LWW";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;

            /*
            {
    "Meta Data": {
        "1. Information": "Intraday (5min) open, high, low, close prices and volume",
        "2. Symbol": "IBM",
        "3. Last Refreshed": "2024-03-12 19:55:00",
        "4. Interval": "5min",
        "5. Output Size": "Compact",
        "6. Time Zone": "US/Eastern"
    },
    "Time Series (Daily)": {
        "2024-03-12 19:55:00": {
            "1. open": "198.0000",
            "2. high": "198.2500",
            "3. low": "198.0000",
            "4. close": "198.2500",
            "5. volume": "194"
        },
        "2024-03-12 19:50:00": {
            "1. open": "198.1500",
            "2. high": "198.2500",
            "3. low": "197.9000",
            "4. close": "198.2500",
            "5. volume": "316"
        },
        "2024-03-12 19:45:00": {
            "1. open": "198.1600",
            "2. high": "198.2400",
            "3. low": "198.0300",
            "4. close": "198.0400",
            "5. volume": "269"
        },
        "2024-03-12 19:40:00": {
            "1. open": "198.2400",
            "2. high": "198.2400",
            "3. low": "198.2400",
            "4. close": "198.2400",
            "5. volume": "20"
        },
            
            */

            // Convert the response to a dictionary
            var stockDataDict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseBody);

            // Extract the time series data
            var timeSeriesData = stockDataDict["Time Series (Daily)"];

            // Create a list to store the stock data
            var stockData = new List<StockModel>();

            // Iterate over the time series data and populate the stock data list
            foreach (var item in timeSeriesData)
            {
                var date = DateTime.Parse(item.Name);
                var open = double.Parse(item.Value["1. open"].Value);
                var high = double.Parse(item.Value["2. high"].Value);
                var low = double.Parse(item.Value["3. low"].Value);
                var close = double.Parse(item.Value["4. close"].Value);
                var volume = int.Parse(item.Value["5. volume"].Value);

                var stockModel = new StockModel
                {
                    Date = date,
                    Open = open,
                    High = high,
                    Low = low,
                    Close = close,
                    Volume = volume
                };

                stockData.Add(stockModel);
            }

            return stockData;

            





            // TODO: Replace with actual data retrieval logic
            // var stockData = new List<StockModel>
            // {
            //     new StockModel { Date = DateTime.Parse("2000/01/04"), Open = 39.88M, High = 41.12M, Low = 39.75M, Close = 40.12M, Volume = 3584700 }
            // };

            // return stockData;
        }
    }
}
