using Lab1;
using Lab4_server;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private readonly IPAddress _ipAddress;
    private readonly int _port;

    public readonly TcpListener _listener;

    const int N = 200;
    const int R = 10;

    public Server()
    {
        _ipAddress = IPAddress.Parse("127.0.0.1");
        _port = 6666;
        _listener = new TcpListener(_ipAddress, _port);
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Server is started and ready to receive calls...");
    }

    public void ShutDown()
    { 
        _listener.Stop();
        Console.WriteLine("Server is shut down.");
    }

    public TcpClient AcceptClient()
    {
        Console.WriteLine("Client connected!");
        return _listener.AcceptTcpClient();
    }

    public void ReleaseClient(TcpClient client)
    {
        client.Close();
        Console.WriteLine("Client disconnected from server. \n");
    }

    public string PerformCalculations(byte[] data)
    {
        Console.WriteLine("Server started the calculations...");
        return Lab4_server.Services.Calculate(data, R);
    }

    static void Main(string[] args)
    {
        Server server = new Server();
        server.Start();

        while (true)
        {
            var client = server.AcceptClient();

            byte[] data = new byte[N];

            NetworkStream stream = client.GetStream();
            stream.Read(data, 0, data.Length);

            var result = server.PerformCalculations(data);

            byte[] resultBytes = Encoding.ASCII.GetBytes(result);
            stream.Write(resultBytes, 0, resultBytes.Length);
            Console.WriteLine("Sent results to the client.");
        }
    }
}
