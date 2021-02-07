using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter
{
    class MainClass
    {
        private const string URL = "https://free.currencyconverterapi.com";
        private const string API_KEY = "bad232ddd5f3d59564e9";
        private static string[] currencyCodes = { "AED", "AFN", "ALL", "AMD", "ANG",
            "AOA", "ARS", "AUD", "AWG", "AZN", "BAM", "BBD", "BDT", "BGN", "BHD",
            "BIF", "BMD", "BND", "BOB", "BOV", "BRL", "BSD", "BTN", "BWP", "BYR",
            "BZD", "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP",
            "COU", "CRC", "CUC", "CUP", "CVE", "CZK", "DJF", "DKK", "DOP", "DZD",
            "EGP", "ERN", "ETB", "EUR", "FJD", "FKP", "GBP", "GEL", "GHS", "GIP",
            "GMD", "GNF", "GTQ", "GYD", "HKD", "HNL", "HRK", "HTG", "HUF", "IDR",
            "ILS", "INR", "IQD", "IRR", "ISK", "JMD", "JOD", "JPY", "KES", "KGS",
            "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT", "LAK", "LBP", "LKR",
            "LRD", "LSL", "LTL", "LVL", "LYD", "MAD", "MDL", "MGA", "MKD", "MMK",
            "MNT", "MOP", "MRO", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZN",
            "NAD", "NGN", "NIO", "NOK", "NPR", "NZD", "OMR", "PAB", "PEN", "PGK",
            "PHP", "PKR", "PLN", "PYG", "QAR", "RON", "RSD", "RUB", "RWF", "SAR",
            "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLL", "SOS", "SRD", "SSP",
            "STD", "SYP", "SZL", "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD",
            "TWD", "TZS", "UAH", "UGX", "USD", "USN", "USS", "UYI", "UYU", "UZS",
            "VEF", "VND", "VUV", "WST", "XAF", "XAG", "XAU", "XBA", "XBB", "XBC",
            "XBD", "XCD", "XDR", "XFU", "XOF", "XPD", "XPF", "XPT", "XTS", "XXX",
            "YER", "ZAR", "ZMW"};


  
        public static void Main(string[] args)
        {
            string currencyFrom = "";
            string currencyTo = "";
            decimal amount = 0;
            bool isDecimal = false;
            bool isValidCurrency = false;

            // Loop until a valid currency is entered
            while (!isValidCurrency)
            {
                Console.WriteLine("Which currency would you like to convert from?");
                currencyFrom = Console.ReadLine().ToUpper();

                // Check if input is a valid currency
                if (currencyCodes.Contains(currencyFrom))
                {
                    isValidCurrency = true;
                } else
                {
                    Console.WriteLine("\nInvalid currency, please try again.");
                }
            }

            isValidCurrency = false;

            // Loop until a valid currency is entered
            while (!isValidCurrency)
            {
                Console.WriteLine("\nWhich currency would you like to convert to?");
                currencyTo = Console.ReadLine().ToUpper();

                // Check if input is a valid currency
                if (currencyCodes.Contains(currencyTo))
                {
                    isValidCurrency = true;
                } else
                {
                    Console.WriteLine("\nInvalid currency, please try again.");
                }
            }

            // Loop until a valid format is entered (i.e. decimal)
            while (!isDecimal)
            {
                Console.WriteLine("\nHow much " + currencyFrom + " would you like to convert to " + currencyTo + "?");

                //Check to see if user input is a valid format (i.e. decimal)
                isDecimal = decimal.TryParse(Console.ReadLine(), out amount);

                if (!isDecimal)
                {
                    Console.WriteLine("\nInvalid format please try again.");
                }
            }

            decimal value = ConvertCurrency(currencyFrom, currencyTo, amount);

            Console.WriteLine("\n" + currencyFrom + " " + amount + " = " + currencyTo + " " + value);
        }

        /// <summary>
        /// Function that returns the converted currency amount
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <param name="amount"></param>
        /// <returns>Converted currency amount</returns>
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

                    // Call API and store the response
                    var response = await client.GetAsync($"/api/v6/convert?q={currencyFrom}_{currencyTo}&compact=y&apiKey={API_KEY}");

                    // Get result from API as a string
                    var stringResult = await response.Content.ReadAsStringAsync();

                    return stringResult;
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException.StackTrace);
                    return "Error calling API. Please do manual lookup.";

                }
            }
        }
    }
}