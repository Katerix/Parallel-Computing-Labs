using System.Net.Sockets;
using System.Net;
using Lab1;

class Client
{
    static void Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 6666;

        while (true)
        {
            TcpClient client = new TcpClient("localhost", port);
            Console.WriteLine("Connected to server!");

            var message = Services.RandomInit(100);

            NetworkStream stream = client.GetStream();
            stream.Write(message, 0, message.Length);
            Console.WriteLine("Message sent!");
            client.Close();

            Console.WriteLine("Disconnected from server.");

        }
    }
}
