using System.Net.Sockets;
using System.Net;
using Lab1;
using System.Text;
using Lab4_client;
using System.IO;
using System.Xml.Linq;

class Client
{
    const int N = 10000; // array size
    const int R = 10; // range
    const int THREAD_AMOUNT = 4;

    private int CONFIG_SIZE = 6 + THREAD_AMOUNT.ToString().Length + N.ToString().Length + R.ToString().Length;

    public readonly int _port = 6666;

    public Client()
    { 
        _port = 6666;
    }

    public void ClientMethod()
    {
        var name = $"Client {Thread.CurrentThread.ManagedThreadId}";

        while (true)
        {
            TcpClient client = new TcpClient("127.0.0.1", _port);

            NetworkStream stream = client.GetStream();

            byte[] outputBuffer; int bytesRead; string resultString;

            while (true)
            {
                // check if server is ready

                outputBuffer = new byte[9];
                bytesRead = stream.Read(outputBuffer, 0, outputBuffer.Length);
                resultString = Encoding.ASCII.GetString(outputBuffer, 0, bytesRead);

                if (resultString.Contains("connected"))
                {
                    Console.WriteLine($"{name} is connected to server!!!\n");
                    break;
                }
            }

            // prepare configurations

            string config = $"T={THREAD_AMOUNT}N={N}R={R}";

            stream = client.GetStream();
            stream.Write(Encoding.ASCII.GetBytes(config), 0, CONFIG_SIZE);
            
            Console.WriteLine($"{name} sent configurations!\n");

            // check if server received configs

            while (true)
            {
                outputBuffer = new byte[16];
                bytesRead = stream.Read(outputBuffer, 0, outputBuffer.Length);
                resultString = Encoding.ASCII.GetString(outputBuffer, 0, bytesRead);

                if (resultString.Contains("received configs"))
                {
                    Console.WriteLine($"{name}: server received configurations and ready to except data.\n");
                    break;
                }
            }

            // prepare data

            var data = Lab1.Services.RandomInit(R, N).ToArray();
            var inputBuffer = ClientService.ConvertIntArrayToByteArray(data);

            stream = client.GetStream();
            stream.Write(inputBuffer, 0, inputBuffer.Length);

            // check if server received data

            while (true)
            {
                outputBuffer = new byte[4 * N];
                bytesRead = stream.Read(outputBuffer, 0, outputBuffer.Length);
                resultString = Encoding.ASCII.GetString(outputBuffer, 0, bytesRead);

                if (resultString.Contains("received data"))
                {
                    Console.WriteLine($"{name}: server received the data and ready to proceed.\n");
                    break;
                }
            }

            // ping server to start

            var message = Encoding.ASCII.GetBytes("start calculation");
            stream = client.GetStream();
            stream.Write(message, 0, message.Length);

            while (true)
            {
                // get status

                message = Encoding.ASCII.GetBytes("get status");
                stream = client.GetStream();
                stream.Write(message, 0, message.Length);

                outputBuffer = new byte[50];
                bytesRead = stream.Read(outputBuffer, 0, outputBuffer.Length);
                resultString = Encoding.ASCII.GetString(outputBuffer, 0, bytesRead);

                if (resultString.Contains("results"))
                { 
                    break;
                }
            }

            Console.WriteLine($"{name} received: {resultString}");

            // ping server thst results received

            message = Encoding.ASCII.GetBytes("received");
            stream = client.GetStream();
            stream.Write(message, 0, message.Length);

            Console.WriteLine($"{name} disconnected from server.\n");
            client.Close();

            Thread.Sleep(2000);
        }
    }

    static void Main(string[] args)
    {
        /*Client[] clients = new Client[4];

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
        }*/

        Client client = new Client();
        client.ClientMethod();
    }
}
