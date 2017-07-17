using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ACW_08346_483446_ServiceLibrary
{
    //This is the server
    public class Service : IService
    {
        //This is a member varibale used in the Server
        private static RSAParameters rsaParameters;

        //This initialises the Server
        public static void InitialiseServer()
        {
            //This increases the size of the buffer for the console read line method
            Console.SetIn(new StreamReader(Console.OpenStandardInput(1024), Encoding.ASCII, false, 1024));

            //This initialises the member variable of the Server
            rsaParameters.D = HexStringToByteArray(Console.ReadLine().ToUpper());
            rsaParameters.DP = HexStringToByteArray(Console.ReadLine().ToUpper());
            rsaParameters.DQ = HexStringToByteArray(Console.ReadLine().ToUpper());
            rsaParameters.Exponent = HexStringToByteArray(Console.ReadLine().ToUpper());
            rsaParameters.InverseQ = HexStringToByteArray(Console.ReadLine().ToUpper());
            rsaParameters.Modulus = HexStringToByteArray(Console.ReadLine().ToUpper());
            rsaParameters.P = HexStringToByteArray(Console.ReadLine().ToUpper());
            rsaParameters.Q = HexStringToByteArray(Console.ReadLine().ToUpper());
        }

        //This is the interface item for the HELLO method of the server
        public string HELLO(int id)
        {
            //This displays the hello response from the server
            Console.Write("Client No. {0} has contacted the server.\r\n", id);

            //This sends the hello response to the client
            return "Hello";
        }

        //This is the interface item for the SORT method of the server
        public string SORT(string[] value)
        {
            Array.Sort(value);

            string message = value[0];

            for (int i = 1; i < value.Length; i++)
            {
                message += " " + value[i];
            }

            //This displays the sort response from the server
            Console.Write("Sorted values:\r\n{0}\r\n", message);

            //This sends the sort response to the client
            return message;
        }

        //This is the interface item for the PUBKEY method of the server
        public byte[][] PUBKEY()
        {
            //This displays the public key response from the server
            Console.Write("Sending the public key to the client.\r\n");

            //This sends the public key response to the client
            return new byte[][] { rsaParameters.Exponent, rsaParameters.Modulus };
        }

        //This is the interface item for the ENC method of the server
        public void ENC(byte[] message)
        {
            try
            {
                //This displays the encryption response from the server
                Console.Write("Decrypted message is: {0}.\r\n", Encoding.ASCII.GetString(Decrypt(message, rsaParameters)));
            }
            catch (CryptographicException e)
            {
                //This displays any cryptographic exceptions
                Console.Write(e.Message);
            }
        }

        //This is the interface item for the SHA1 method of the server
        public string SHA1(string inMessage)
        {
            using (SHA1 sha1 = new SHA1CryptoServiceProvider())
            {
                string message = ByteArrayToHexString(sha1.ComputeHash(Encoding.ASCII.GetBytes(inMessage)));

                //This displays the hash response from the server
                Console.Write("SHA-1 hash of {0} is {1}.\r\n", inMessage, message);

                //This sends the hash response to the client
                return message;
            }
        }

        //This is the interface item for the SHA256 method of the server
        public string SHA256(string inMessage)
        {
            using (SHA256 sha256 = new SHA256CryptoServiceProvider())
            {
                string message = ByteArrayToHexString(sha256.ComputeHash(Encoding.ASCII.GetBytes(inMessage)));

                //This displays the hash response from the server
                Console.Write("SHA-256 hash of {0} is {1}.\r\n", inMessage, message);

                //This sends the hash response to the client
                return message;
            }
        }

        //This is the interface item for the SIGN method of the server
        public byte[] SIGN(string inMessage)
        {
            byte[] message = null;

            //If the message is not any permutation of cheesecake
            if (inMessage.ToUpper() != "CHEESECAKE")
            {
                try
                {
                    message = Sign(Encoding.ASCII.GetBytes(inMessage), rsaParameters);

                    //This displays the sign response from the server
                    Console.Write("Signing data: {0}.\r\n", inMessage);
                }
                catch (CryptographicException e)
                {
                    //This displays any cryptographic exceptions
                    Console.Write(e.ToString());
                }
            }
            else
            {
                Console.Write("No cheesecake allowed.\r\n");
            }

            //This sends the sign response to the client
            return message;
        }

        //This converts from a string of hex characters to a byte array
        private static byte[] HexStringToByteArray(string hexString)
        {
            return Enumerable.Range(0, hexString.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hexString.Substring(x, 2), 16)).ToArray();
        }

        //This converts from a byte array to a string of hex characters
        private static string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = string.Empty;

            if (byteArray != null)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    hexString += byteArray[i].ToString("x2");
                }
            }

            return hexString;
        }

        //This decrypts a message
        private static byte[] Decrypt(byte[] dataToDecrypt, RSAParameters rsaParameters)
        {
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.ImportParameters(rsaParameters);

                return rsaCryptoServiceProvider.Decrypt(dataToDecrypt, false);
            }
        }

        //This signs a message
        private static byte[] Sign(byte[] dataToSign, RSAParameters rsaParameters)
        {
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            using (SHA256CryptoServiceProvider sha256CryptoServiceProvider = new SHA256CryptoServiceProvider())
            {
                rsaCryptoServiceProvider.ImportParameters(rsaParameters);

                return rsaCryptoServiceProvider.SignData(dataToSign, sha256CryptoServiceProvider);
            }
        }
    }
}