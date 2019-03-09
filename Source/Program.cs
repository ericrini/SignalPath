using System;

namespace SignalPath
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Expected an input parameter.");
            }


            Console.WriteLine(Convert.HexToBase64(args[0]));
        }
    }
}
