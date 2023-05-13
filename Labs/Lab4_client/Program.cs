using System.Net.Sockets;
using System.Net;
using Lab1;
using System.Text;
using Lab4_client;

class Client
{
    const int N = 200;
    const int R = 10;

    static void Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 6666;

        while (true)
        {
            TcpClient client = new TcpClient("localhost", port);
            Console.WriteLine("Connected to server!");

            var message = ClientService.ConvertIntArrayToByteArray(Lab1.Services.RandomInit(R, N).ToArray());

            NetworkStream stream = client.GetStream();
            stream.Write(message, 0, message.Length);
            Console.WriteLine("Message sent!");

            stream = client.GetStream();
            stream.Read(message, 0, message.Length);

            Console.WriteLine(Encoding.ASCII.GetString(message));
            client.Close();

            Console.WriteLine("Disconnected from server.");

        }
    }
}
