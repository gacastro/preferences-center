using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Code.Helpers;

namespace Code
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("*** Hello, thank you for using the most amazing contact preferences application ***");

            var argumentParser = GetArgumentParser(args);
            if (argumentParser == null)
                return 1;

            IEnumerable outputToPrint;
            try
            {
                var lines = File.ReadAllLines(argumentParser.FilePath);
                var customers = new CustomerParser(lines).Customers;

                outputToPrint = 
                    new OutputBuilder(
                            DateTimeOffset.UtcNow, 
                            customers,
                            90)
                        .Build();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Oops something went wrong:");
                Console.WriteLine($"=> {exception.Message}");
                return 1;
            }

            Console.WriteLine("Here is the list of customers to send preferences, grouped by day:");
            foreach (var output in outputToPrint)
                Console.WriteLine($"=> {output}");

            return 0;
        }
        
        private static ArgumentParser GetArgumentParser(IReadOnlyList<string> args)
        {
            var argumentParser = new ArgumentParser(args);

            if (argumentParser.ErrorMessages.Count == 0)
                return argumentParser;
            
            Console.WriteLine("Oops something went wrong:");
            foreach (var errorMessage in argumentParser.ErrorMessages)
            {
                Console.WriteLine($"=> {errorMessage}");
            }

            return null;
        }
    }
}