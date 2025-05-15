using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CTcpServer;

public class ChatServerExample
{
    public static void Run()
    {
        using var listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine("Server started on port 5000.");

        while (true)
        {
            var client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            var stream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                var received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: " + received);

                var response = $"Received '{received}' at {DateTime.Now}";
                var responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }

            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}