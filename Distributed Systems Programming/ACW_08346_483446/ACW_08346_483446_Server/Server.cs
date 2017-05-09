using ACW_08346_483446_ServiceLibrary;
using System;
using System.ServiceModel;
using System.Threading;

namespace ACW_08346_483446_Server
{
    class Server
    {
        static void Main(string[] args)
        {
            InitialiseServer();

            OpenServer();
        }

        private static void InitialiseServer()
        {
            Service.InitialiseServer();
        }

        private static void OpenServer()
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Service));

            serviceHost.Open();
            
            RunServer(serviceHost);
        }

        private static void RunServer(ServiceHost serviceHost)
        {
            Console.WriteLine("Server running...");

            CloseServer(serviceHost);
        }

        private static void CloseServer(ServiceHost serviceHost)
        {
            Thread.Sleep(9000);

            serviceHost.Close();
        }
    }
}