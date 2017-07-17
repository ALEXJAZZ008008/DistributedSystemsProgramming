using ACW_08346_483446_ServiceLibrary;
using System;
using System.ServiceModel;
using System.Threading;

namespace ACW_08346_483446_Server
{
    //This is the server
    class Server
    {
        //This is the Main method of the Server
        static void Main(string[] args)
        {
            //This initialises the Server
            InitialiseServer();

            //This opens the server
            OpenServer();
        }

        //This initialises the Server
        private static void InitialiseServer()
        {
            Service.InitialiseServer();
        }

        //This opens the server
        private static void OpenServer()
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Service));

            serviceHost.Open();

            //This runs the server
            RunServer(serviceHost);
        }

        //This runs the server
        private static void RunServer(ServiceHost serviceHost)
        {
            Console.WriteLine("Server running...");

            //This closes the server
            CloseServer(serviceHost);
        }

        //This closes the server
        private static void CloseServer(ServiceHost serviceHost)
        {
            Thread.Sleep(9000);

            //This closes the server
            serviceHost.Close();
        }
    }
}