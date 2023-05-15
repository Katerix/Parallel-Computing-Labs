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

            WriteToStream(client, stream, "connected");

            stream = client.GetStream();
            var input = ReadInput(client, stream);

            var result = PerformCalculations(input);

            SendResults(client, stream, result);
        }

        public static string PerformCalculations(byte[] data)
        {
            Console.WriteLine($"Server started the calculations for client {Thread.CurrentThread.Name}...\n"); //no
            return Lab4_server.Services.Calculate(data, R);
        }

        public static void SendResults(TcpClient client, NetworkStream stream, string result)
        {
            stream = client.GetStream();
            byte[] resultBytes = Encoding.ASCII.GetBytes(result);
            stream.Write(resultBytes, 0, resultBytes.Length);
            Console.WriteLine($"Sent results to the client {Thread.CurrentThread.Name}.\n"); //no
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
            byte[] data = new byte[N];
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
