using System.Net.Sockets;
using System.Text;

namespace CTcpClient;

internal class ChatClientExample
{
    internal static void Run()
    {
        using var client = new TcpClient("127.0.0.1", 5000);
        using var stream = client.GetStream();

        Console.WriteLine("Connected to server. Type messages or 'exit' to quit.");

        while (true)
        {
            Console.Write("You: ");
            var input = Console.ReadLine();
            if (input == "exit") break;

            var messageBytes = Encoding.UTF8.GetBytes(input);
            stream.Write(messageBytes, 0, messageBytes.Length);

            var buffer = new byte[1024];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine("Server: " + response);
        }

        client.Close();
        Console.WriteLine("Connection closed.");
    }
}