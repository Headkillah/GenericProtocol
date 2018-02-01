using GenericProtocolTestLib;
using System;

namespace GenericProtocolServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GPTestLibMain.Test(false);

            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}