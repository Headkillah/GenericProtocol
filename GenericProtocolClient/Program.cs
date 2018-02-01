using GenericProtocolTestLib;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GenericProtocolClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GPTestLibMain.Test(true);

            string str = new string(GenerateString(11).ToArray());

            while (true)
            {
                Thread.Sleep(10);
                GPTestLibMain.SendToServer(str);
            }
        }

        private static IEnumerable<char> GenerateString(int len)
        {
            for (int i = 0; i < len; ++i)
                yield return 'a';
        }
    }
}