using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_server
{
    public static class WorkerThread
    {
        public static void HandleClient(TcpClient client)
        {
            int N, R, threadAmount;

            NetworkStream stream = client.GetStream();

            WriteToStream(client, stream, "connected");
            Console.WriteLine("Connected");

            byte[] inputArray; string message;

            
            inputArray = ReadInput(client, stream);
            var testVariable = Encoding.ASCII.GetString(inputArray, 0, 15);

            if (int.TryParse(testVariable, out _))
            {
                WriteToStream(client, stream, "received data");
                Console.WriteLine("Received array");
            }
            

            while (true)
            {
                message = ReadFromStream(client, stream);

                if (message.Contains("start calculation"))
                {
                    Console.WriteLine("Starting calculation...");

                    var calculationThread = Task.Run(() => PerformCalculations(inputArray, R, threadAmount));

                    while (!calculationThread.IsCompleted)
                    {
                        message = ReadFromStream(client, stream);

                        if (message != null && message.Contains("get status"))
                        {
                            WriteToStream(client, stream, "in progress");
                        }
                    }

                    SendResults(client, stream, calculationThread.Result);
                    break;
                }
            }
        }

        public static string PerformCalculations(byte[] data, int range, int threadAmount)
        {
            // Console.WriteLine($"Server started the calculations for client {Thread.CurrentThread.Name}...\n"); //no
            return Lab4_server.Services.Calculate(data, range, threadAmount);
        }

        public static void SendResults(TcpClient client, NetworkStream stream, string result)
        {
            stream = client.GetStream();
            byte[] resultBytes = Encoding.ASCII.GetBytes(result);
            stream.Write(resultBytes, 0, resultBytes.Length);
            // Console.WriteLine($"Sent results to the client {Thread.CurrentThread.Name}.\n"); //no
        }

        public static byte[] ReadInput(TcpClient client, NetworkStream stream)
        {
            stream = client.GetStream();
            byte[] data = new byte[N];
            stream.Read(data, 0, data.Length);

            return data;
        }

        public static string ReadFromStream(TcpClient client, NetworkStream stream)
        {
            stream = client.GetStream();
            byte[] data = new byte[20];
            stream.Read(data, 0, data.Length);
            return Encoding.ASCII.GetString(data);
        }

        public static void WriteToStream(TcpClient client, NetworkStream stream, string message)
        {
            stream = client.GetStream();
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
