using System;
using ACW_08346_483446_Client.ServiceReference;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

namespace ACW_08346_483446_Client
{
    class Client
    {
        private static ServiceClient serviceClient;
        private static RSAParameters rsaParameters;

        static void Main(string[] args)
        {
            InitialiseClient();

            RunClient();
        }

        private static void InitialiseClient()
        {
            serviceClient = new ServiceClient();
        }

        private static void RunClient()
        {
            for(int i = 0; i < int.Parse(Console.ReadLine()); i++)
            {
                string[] message = Console.ReadLine().Split(new char[] { ' ' }, 2);

                switch(message[0].ToUpper())
                {
                    case "HELLO":

                        HELLO(message[1]);

                        break;

                    case "SORT":

                        SORT(message[1]);

                        break;

                    case "PUBKEY":

                        PUBKEY();

                        break;

                    case "ENC":

                        ENC(message[1]);

                        break;

                    case "SHA1":

                        SHA1(message[1]);

                        break;

                    case "SHA256":

                        SHA256(message[1]);

                        break;

                    case "SIGN":

                        SIGN(message[1]);

                        break;
                }
            }
        }

        private static void HELLO(string inMessage)
        {
            Console.Write("{0}\r\n", serviceClient.HELLO(int.Parse(inMessage)));
        }

        private static void SORT(string inMessage)
        {
            string[] message = inMessage.Split(new char[] { ' ' }, 2);

            int number = int.Parse(message[0]);
            message = message[1].Split(' ');

            Console.Write("Sorted values:\r\n{0}\r\n", serviceClient.SORT(number, message));
        }

        private static void PUBKEY()
        {
            string[] PUBKEY = serviceClient.PUBKEY().Split(new char[] { ',' }, 2);

            rsaParameters.Exponent = StringToByteArray(PUBKEY[0]);
            rsaParameters.Modulus = StringToByteArray(PUBKEY[1]);

            Console.Write("{0}\r\n{1}\r\n", rsaParameters.Exponent, rsaParameters.Modulus);
        }

        private static void ENC(string inMessage)
        {
            if(rsaParameters.Exponent != null && rsaParameters.Modulus != null)
            {
                try
                {
                    serviceClient.ENC(ByteArrayToHexString(RSAEncrypt(Encoding.Unicode.GetBytes(inMessage), rsaParameters)));

                    Console.Write("Encrypted message sent.\r\n");
                }
                catch(CryptographicException e)
                {
                    Console.Write(e.ToString());
                }
            }
            else
            {
                Console.Write("No public key.\r\n");
            }
        }

        private static void SHA1(string inMessage)
        {
            Console.Write("{0}\r\n", serviceClient.SHA1(inMessage));
        }

        private static void SHA256(string inMessage)
        {
            Console.Write("{0}\r\n", serviceClient.SHA256(inMessage));
        }

        private static void SIGN(string inMessage)
        {
            if (rsaParameters.Exponent != null && rsaParameters.Modulus != null)
            {
                try
                {
                    serviceClient.SIGN(inMessage);

                }
                catch (CryptographicException e)
                {
                    Console.Write(e.ToString());
                }
            }
            else
            {
                Console.Write("No public key.\r\n");
            }
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo)
        {
            try
            {
                byte[] encryptedData;

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }

                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        static string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = "";

            if (null != byteArray)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    hexString += byteArray[i].ToString("x2");
                }
            }

            return hexString;
        }
    }
}