using System.Net.Sockets;
using System.Net;
using Lab1;
using System.Text;
using Lab4_client;

class Client
{
    const int N = 10000;
    const int R = 10;

    public readonly int _port = 6666;

    public Client()
    { 
        _port = 6666;
    }

    public void ClientMethod()
    {
        var name = $"Client {Thread.CurrentThread.Name}";

        while (true)
        {
            TcpClient client = new TcpClient("localhost", _port);
            Console.WriteLine($"{name} is connected to server!\n");

            var data = Lab1.Services.RandomInit(R, N).ToArray();
            var inputBuffer = ClientService.ConvertIntArrayToByteArray(data);

            NetworkStream stream = client.GetStream();
            stream.Write(inputBuffer, 0, inputBuffer.Length);
            
            Console.WriteLine($"{name} sent input data!\n");


            byte[] outputBuffer = new byte[1024];
            int bytesRead = stream.Read(outputBuffer, 0, outputBuffer.Length);
            string resultString = Encoding.ASCII.GetString(outputBuffer, 0, bytesRead);

            Console.WriteLine($"{name} received: {resultString}");
            client.Close();

            Console.WriteLine($"{name} disconnected from server.\n");
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

        for (int i = 0; i < 4; i++)
        {
            int index = i;

            threads[i] = new Thread(() => clients[index].ClientMethod());
            threads[i].Name = (index + 1).ToString();
            threads[i].Start();
        }

        foreach (var thr in threads)
        {
            thr.Join();
        }
    }
}
