using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CurrencyConverter
{
    class MainClass
    {

        private const string URL = "https://free.currencyconverterapi.com";
        private const string API_KEY = "bad232ddd5f3d59564e9";

  
        public static void Main(string[] args)
        {
            string currencyFrom;
            string currencyTo;
            decimal amount = 0;
            bool isDecimal = false;

          
            Console.WriteLine("Which currency would you like to convert from?");
            currencyFrom = Console.ReadLine();

            Console.WriteLine("\nWhich currency would you like to convert to?");
            currencyTo = Console.ReadLine();

            // Run loop until a valid format is entered (i.e. decimal)
            while (!isDecimal)
            {
                Console.WriteLine("\nHow much " + currencyFrom + " would you like to convert to " + currencyTo + "?");

                //Check to see if entered value is a valid format (i.e. decimal)
                isDecimal = decimal.TryParse(Console.ReadLine(), out amount);

                if (!isDecimal)
                {
                    Console.WriteLine("\nInvalid format please try again.");
                }
            }

            decimal value = ConvertCurrency(currencyFrom, currencyTo, amount);

            Console.WriteLine("\nThe total is " + currencyTo + " " + value);
        }

        /// <summary>
        /// Function that returns the converted currency amount
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <param name="amount"></param>
        /// <returns>"convertedAmount"</returns>
        public static decimal ConvertCurrency(string currencyFrom, string currencyTo, decimal amount)
        {
            decimal exchangeRate = GetExchangeRate(currencyFrom, currencyTo);

            // Calculate the converted currency
            decimal convertedAmount = amount * exchangeRate;

            // Round to the nearest hundredth decimal place.
            convertedAmount = Math.Round(convertedAmount, 2);

            return convertedAmount;
        }

        /// <summary>
        /// Function that finds the exchange rate based on currencyFrom and currencyTo
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <returns>The exchange rate as a decimal</returns>
        private static decimal GetExchangeRate(string currencyFrom, string currencyTo)
        {

            var result = CallExchangeRateAPI(currencyFrom, currencyTo).Result;

            // Save result to a dictionary
            var dictResult = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(result);

            // Get exchange rate value in dictionary based on provided currencyFrom and currencyTo 
            decimal exchangeRate = decimal.Parse(dictResult[$"{currencyFrom}_{currencyTo}"]["val"]);

            return exchangeRate;
           
        }

        /// <summary>
        /// Function that calls API to get real-time exchange rate data
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <returns>The exchange rate as a string</returns>
        private static async Task<string> CallExchangeRateAPI(string currencyFrom, string currencyTo)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(URL);
                    var response = await client.GetAsync($"/api/v6/convert?q={currencyFrom}_{currencyTo}&compact=y&apiKey={API_KEY}");
                    var stringResult = await response.Content.ReadAsStringAsync();

                    return stringResult;
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    Environment.Exit(1);
                    return "Error calling API. Please do manual lookup.";

                }
            }
        }
    }
}