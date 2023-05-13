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

            var data = Lab1.Services.RandomInit(R, N).ToArray();
            var inputBuffer = ClientService.ConvertIntArrayToByteArray(data);

            NetworkStream stream = client.GetStream();
            stream.Write(inputBuffer, 0, inputBuffer.Length);
            ClientService.PrintArray(data);
            Console.WriteLine("Input data sent!");


            byte[] outputBuffer = new byte[1024];
            int bytesRead = stream.Read(outputBuffer, 0, outputBuffer.Length);
            string resultString = Encoding.ASCII.GetString(outputBuffer, 0, bytesRead);

            Console.WriteLine($"Received: {resultString}");
            client.Close();

            Console.WriteLine("Disconnected from server.\n");
        }
    }
}
