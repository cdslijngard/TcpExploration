using System.Net.Sockets;
using System.Text;

namespace CTcpClient;

public class CTcpClient
{
    public static void Run()
    {
        var id = Guid.NewGuid();

        for (var times = 0; times < 10; times++)
        {
            Thread.Sleep(1000);

            var client = new TcpClient("127.0.0.1", 5000);

            var stream = client.GetStream();
            var message = $"Client id {id}: Message #{times + 1}";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            stream.Write(messageBytes, 0, messageBytes.Length);

            var buffer = new byte[1024];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine("Server response: " + response);

            client.Close();
        }

        Console.WriteLine("Tasks done.");
    }
}