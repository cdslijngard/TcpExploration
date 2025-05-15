using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CTcpServer;

internal class CTcpServer
{
    internal static void Run()
    {
        var listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine("Server started on port 5000.");

        while (true)
        {
            var client = listener.AcceptTcpClient();

            var stream = client.GetStream();

            var buffer = new byte[1024];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            var received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine("Received: " + received);

            var response = $"Message received: {received}";
            var responseBytes = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }
}