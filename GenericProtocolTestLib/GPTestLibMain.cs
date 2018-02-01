using GenericProtocol.Implementation;
using System;
using System.Net;

namespace GenericProtocolTestLib
{
    public static class GPTestLibMain
    {
        private static ProtoServer<string> _server;
        private static ProtoClient<string> _client;

        private static readonly IPAddress ServerIp = IPAddress.Loopback;

        public static void Test(bool launchClient, bool sendText = false)
        {
            //INetworkDiscovery discovery = new NetworkDiscovery();
            //discovery.Host(IPAddress.Any);
            //discovery.Discover();

            if (!launchClient)
                StartServer();
            else
                StartClient();

            Console.WriteLine("\n");

            if (sendText)
                while (true)
                {
                    string text = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(text)) continue;

                    if (launchClient)
                        SendToServer(text);
                    else
                        SendToClients(text);
                }
        }

        private static void StartClient()
        {
            _client = new ProtoClient<string>(ServerIp, 1024) { AutoReconnect = true };
            _client.ReceivedMessage += ClientMessageReceived;
            _client.ConnectionLost += Client_ConnectionLost;

            Console.WriteLine("Connecting");
            _client.Connect().GetAwaiter().GetResult();
            Console.WriteLine("Connected!");
            _client.Send("Hello Server!").GetAwaiter().GetResult();
        }

        public static void SendToServer(string message)
        {
            _client?.Send(message);
        }

        public static void SendToClients(string message)
        {
            _server?.Broadcast(message);
        }

        public static void Client_ConnectionLost(IPEndPoint endPoint)
        {
            Console.WriteLine($"Connection lost! {endPoint.Address}");
        }

        private static void StartServer()
        {
            _server = new ProtoServer<string>(IPAddress.Any, 1024);
            Console.WriteLine("Starting Server...");
            _server.Start();
            Console.WriteLine("Server started!");
            _server.ClientConnected += ClientConnected;
            _server.ReceivedMessage += ServerMessageReceived;
        }

        public static async void ServerMessageReceived(IPEndPoint sender, string message)
        {
            Console.WriteLine($"{sender}: {message}");
            await _server.Send($"Hello {sender}!", sender);
        }

        public static void ClientMessageReceived(IPEndPoint sender, string message)
        {
            Console.WriteLine($"{sender}: {message}");
        }

        public static async void ClientConnected(IPEndPoint address)
        {
            await _server.Send($"Hello {address}!", address);
        }
    }
}