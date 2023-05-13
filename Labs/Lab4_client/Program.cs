using System.Net.Sockets;
using System.Net;
using Lab1;
using System.Text;
using Lab4_client;

class Client
{
    const int N = 200;
    const int R = 10;

    private readonly IPAddress _ipAddress = IPAddress.Parse("127.0.0.1");
    public readonly int _port = 6666;

    public Client()
    {
        _ipAddress = IPAddress.Parse("127.0.0.1");
        _port = 6666;
    }

    public void ClientMethod()
    {
        while (true)
        {
            TcpClient client = new TcpClient("localhost", _port);
            Console.WriteLine($"Client {Thread.CurrentThread.Name} is connected to server!");

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

            Console.WriteLine($"Client {Thread.CurrentThread.Name} disconnected from server.\n");
        }
    }

    static void Main(string[] args)
    {
        Client[] clients = new Client[4];

        for (int i = 0; i < 4; i++)
        {
            clients[i] = new Client();
        }

        Thread[] threads = new Thread[4];

        for (int i = 0; i < 3; i++)
        {
            threads[i] = new Thread(() => clients[i].ClientMethod());
            threads[i].Name = (i + 1).ToString();
            threads[i].Start();
        }

        foreach (var thr in threads)
        {
            thr.Join();
        }
    }
}
