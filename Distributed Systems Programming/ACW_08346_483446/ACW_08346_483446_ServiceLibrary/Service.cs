using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ACW_08346_483446_ServiceLibrary
{
    public class Service : IService
    {
        private static RSAParameters rsaParameters;

        public static void InitialiseServer()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput(1024), Encoding.UTF8, false, 1024));

            rsaParameters.D = StringToByteArray(Console.ReadLine());
            rsaParameters.DP = StringToByteArray(Console.ReadLine());
            rsaParameters.DQ = StringToByteArray(Console.ReadLine());
            rsaParameters.Exponent = StringToByteArray(Console.ReadLine());
            rsaParameters.InverseQ = StringToByteArray(Console.ReadLine());
            rsaParameters.Modulus = StringToByteArray(Console.ReadLine());
            rsaParameters.P = StringToByteArray(Console.ReadLine());
            rsaParameters.Q = StringToByteArray(Console.ReadLine());
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public string HELLO(int id)
        {
            Console.Write("Client No. {0} has contacted the server.\r\n", id);

            return "Hello";
        }

        public string SORT(int number, string[] value)
        {
            return string.Empty;
        }

        public string PUBKEY()
        {
            return string.Empty;
        }

        public void ENC(string message)
        {
            
        }

        public string SHA1(string message)
        {
            return string.Empty;
        }

        public string SHA256(string message)
        {
            return string.Empty;
        }

        public string SIGN(string message)
        {
            return string.Empty;
        }
    }
}