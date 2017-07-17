using System;
using ACW_08346_483446_Client.ServiceReference;
using System.Security.Cryptography;
using System.Text;

namespace ACW_08346_483446_Client
{
    //This is the Client
    class Client
    {
        //These are member varibales used in the Client
        private static ServiceClient serviceClient;
        private static RSAParameters rsaParameters;

        //This is the Main method of the Client
        static void Main(string[] args)
        {
            //This initialises the Client
            InitialiseClient();

            //This runs the Client
            RunClient();
        }

        //This initialises the Client
        private static void InitialiseClient()
        {
            serviceClient = new ServiceClient();
        }

        //This runs the Client
        private static void RunClient()
        {
            //This is the loop counter fo the main loop of the program
            int n = int.Parse(Console.ReadLine());

            for (int i = 0; i < n; i++)
            {
                //This splits the type of request from the message
                string[] message = Console.ReadLine().Split(new char[] { ' ' }, 2);

                //This is a switch on the type of request
                switch (message[0].ToUpper())
                {
                    //This is a HELLO request
                    case "HELLO":

                        HELLO(message[1]);

                        break;

                    //This is a SORT request
                    case "SORT":

                        SORT(message[1]);

                        break;

                    //This is a PUBKEY request
                    case "PUBKEY":

                        PUBKEY();

                        break;

                    //This is a ENC request
                    case "ENC":

                        ENC(message[1]);

                        break;

                    //This is a SHA1 request
                    case "SHA1":

                        SHA1(message[1]);

                        break;

                    //This is a SHA256 request
                    case "SHA256":

                        SHA256(message[1]);

                        break;

                    //This is a SIGN request
                    case "SIGN":

                        SIGN(message[1]);

                        break;
                }
            }
        }

        //This is a HELLO request
        private static void HELLO(string message)
        {
            //This displays the hello response from the server
            Console.Write("{0}\r\n", serviceClient.HELLO(int.Parse(message)));
        }

        //This is a SORT request
        private static void SORT(string message)
        {
            //This displays the sort response from the server
            Console.Write("Sorted values:\r\n{0}\r\n", serviceClient.SORT(message.Split(new char[] { ' ' }, 2)[1].Split(' ')));
        }

        //This is a PUBKEY request
        private static void PUBKEY()
        {
            //This recieves the public key from the server
            byte[][] PUBKEY = serviceClient.PUBKEY();

            //These assign the public key
            rsaParameters.Exponent = PUBKEY[0];
            rsaParameters.Modulus = PUBKEY[1];

            //This displays the public key response from the server
            Console.Write("{0}\r\n{1}\r\n", ByteArrayToHexString(rsaParameters.Exponent), ByteArrayToHexString(rsaParameters.Modulus));
        }

        //This is a ENC request
        private static void ENC(string message)
        {
            //If the servers public key is known
            if (rsaParameters.Exponent != null && rsaParameters.Modulus != null)
            {
                try
                {
                    //This sends the encrypted message to the server
                    serviceClient.ENC(Encrypt(Encoding.ASCII.GetBytes(message), rsaParameters));

                    Console.Write("Encrypted message sent.\r\n");
                }
                catch (CryptographicException e)
                {
                    //This displays any cryptographic exceptions
                    Console.Write(e.Message);
                }
            }
            else
            {
                Console.Write("No public key.\r\n");
            }
        }

        //This is a SHA1 request
        private static void SHA1(string message)
        {
            //This displays the hash response from the server
            Console.Write("{0}\r\n", serviceClient.SHA1(message));
        }

        //This is a SHA256 request
        private static void SHA256(string message)
        {
            //This displays the hash response from the server
            Console.Write("{0}\r\n", serviceClient.SHA256(message));
        }

        //This is a SIGN request
        private static void SIGN(string message)
        {
            //If the servers public key is known
            if (rsaParameters.Exponent != null && rsaParameters.Modulus != null)
            {
                try
                {
                    //If the signiture is correct
                    if (Verify(Encoding.ASCII.GetBytes(message), serviceClient.SIGN(message), rsaParameters))
                    {
                        Console.Write("Signature successfully verified.\r\n");
                    }
                    else
                    {
                        Console.Write("Data not signed.\r\n");
                    }
                }
                catch (CryptographicException e)
                {
                    //This displays any cryptographic exceptions
                    Console.Write(e.Message);
                }
            }
            else
            {
                Console.Write("No public key.\r\n");
            }
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

        //This encrypts a message
        private static byte[] Encrypt(byte[] dataToEncrypt, RSAParameters rsaParameters)
        {
            using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.ImportParameters(rsaParameters);

                return rsaCryptoServiceProvider.Encrypt(dataToEncrypt, false);
            }
        }

        //This verifies a signed message
        private static bool Verify(byte[] dataToVerify, byte[] signedData, RSAParameters rsaParameters)
        {
            if (signedData != null)
            {
                using (RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider())
                using (SHA256CryptoServiceProvider sha256CryptoServiceProvider = new SHA256CryptoServiceProvider())
                {
                    rsaCryptoServiceProvider.ImportParameters(rsaParameters);

                    return rsaCryptoServiceProvider.VerifyData(dataToVerify, sha256CryptoServiceProvider, signedData);
                }
            }

            return false;
        }
    }
}