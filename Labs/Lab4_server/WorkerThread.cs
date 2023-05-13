using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_server
{
    public static class WorkerThread
    {
        const int N = 200;
        const int R = 10;

        public static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            var input = ReadInput(stream);

            var result = PerformCalculations(input);

            SendResults(result, stream);
        }

        public static string PerformCalculations(byte[] data)
        {
            Console.WriteLine($"Server started the calculations for client {Thread.CurrentThread.Name}...");
            return Lab4_server.Services.Calculate(data, R);
        }

        public static void SendResults(string result, NetworkStream stream)
        {
            byte[] resultBytes = Encoding.ASCII.GetBytes(result);
            stream.Write(resultBytes, 0, resultBytes.Length);
            Console.WriteLine($"Sent results to the client {Thread.CurrentThread.Name}.\n");
        }

        public static byte[] ReadInput(NetworkStream stream)
        {
            byte[] data = new byte[N];
            stream.Read(data, 0, data.Length);

            return data;
        }
    }
}
