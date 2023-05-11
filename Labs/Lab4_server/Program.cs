using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    const int N = 10000;
    static void Main(string[] args)
    {
        IPAddress ipAdress = IPAddress.Parse("127.0.0.1");
        int port = 6666;

        var listener = new TcpListener(ipAdress, port);

        listener.Start();

        Console.WriteLine("Server is started and ready to receive calls...");

        while (true)
        {
            var client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected!");

            byte[] data = new byte[N];

            NetworkStream stream = client.GetStream();
            stream.Read(data, 0, data.Length);

            string message = Encoding.ASCII.GetString(data);

            Console.WriteLine(message);

            client.Close();
            Console.WriteLine("Disconnected from server");
        }
    }
}
