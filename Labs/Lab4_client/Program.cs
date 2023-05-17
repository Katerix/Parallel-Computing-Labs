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

    const int CONFIG_SIZE = 4 * (THREAD_AMOUNT + N + R);

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

            // prepare data
            string config = $"{THREAD_AMOUNT}{N}{R}";
            var data = Lab1.Services.RandomInit(R, N).ToArray();
            var inputBuffer = ClientService.ConvertIntArrayToByteArray(data);

            stream = client.GetStream();
            stream.Write(Encoding.ASCII.GetBytes(config), 0, CONFIG_SIZE);
            stream.Write(inputBuffer, CONFIG_SIZE, inputBuffer.Length);
            
            Console.WriteLine($"{name} sent configurations and input data!\n");

            //check if server received data
            while (true)
            {
                outputBuffer = new byte[N];
                bytesRead = stream.Read(outputBuffer, 0, outputBuffer.Length);
                resultString = Encoding.ASCII.GetString(outputBuffer, 0, bytesRead);

                if (resultString.Contains("received data"))
                {
                    Console.WriteLine($"{name}: server received the data and ready to proceed.\n");
                    break;
                }
            }

            //ping server to start
            var message = Encoding.ASCII.GetBytes("start calculation");
            stream = client.GetStream();
            stream.Write(message, 0, message.Length);

            while (true)
            {
                //get status
                outputBuffer = new byte[1024];
                bytesRead = stream.Read(outputBuffer, 0, outputBuffer.Length);
                resultString = Encoding.ASCII.GetString(outputBuffer, 0, bytesRead);

                if (resultString.Contains("results"))
                { 
                    break;
                }

                Thread.Sleep(1000);

                message = Encoding.ASCII.GetBytes("get status");
                stream = client.GetStream();
                stream.Write(message, 0, message.Length);
            }

            Console.WriteLine($"{name} received: {resultString}");
            client.Close();

            //ping server thst results received
            message = Encoding.ASCII.GetBytes("received");
            stream = client.GetStream();
            stream.Write(message, 0, message.Length);

            Console.WriteLine($"{name} disconnected from server.\n");
            client.Close();
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
