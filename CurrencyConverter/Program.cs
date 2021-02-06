using System;

namespace CurrencyConverter
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string currencyFrom;
            string currencyTo;
            decimal amount = 0;
            bool canConvert = false;

          
            Console.WriteLine("Which currency would you like to convert from?");
            currencyFrom = Console.ReadLine();

            Console.WriteLine("\nWhich currency would you like to convert to?");
            currencyTo = Console.ReadLine();

            while (!canConvert)
            {
                Console.WriteLine("\nHow much " + currencyFrom + " would you like to convert to " + currencyTo + "?");

                canConvert = decimal.TryParse(Console.ReadLine(), out amount);

                if (!canConvert)
                {
                    Console.WriteLine("\nInvalid format please try again.");
                }
            }

            //Console.WriteLine("\nConverting " + amount + " " + currencyFrom + " to " + currencyTo);

            ConvertCurrency(currencyFrom, currencyTo, amount);
        }

        public static decimal ConvertCurrency(string currencyFrom, string currencyTo, decimal amount)
        {


            return 0;
        }
    }
}