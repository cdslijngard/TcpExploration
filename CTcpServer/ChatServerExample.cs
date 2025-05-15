using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CTcpServer;

public class ChatServerExample
{
    private readonly List<TcpClient> _connectedClients = [];
    private object _clientLock = new object();

    public void Run()
    {
        var listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine("Server started on port 5000.");

        while (true)
        {
            var client = listener.AcceptTcpClient();
            Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

            // Start handling client in a new thread/task
            Task.Run(() => HandleClient(client));
        }
    }

    private void Broadcast(string message, TcpClient sender)
    {
        var data = Encoding.UTF8.GetBytes(message);

        lock (_clientLock)
        {
            foreach (var client in _connectedClients.ToList()
                         .Where(client => client != sender && client.Connected))
            {
                try
                {
                    client.GetStream().Write(data, 0, data.Length);
                }
                catch
                {
                    _connectedClients.Remove(client);
                    client.Close();
                }
            }
        }
    }


    private void HandleClient(TcpClient client)
    {
        lock (_clientLock)
        {
            _connectedClients.Add(client);
        }

        var stream = client.GetStream();
        var buffer = new byte[1024];

        try
        {
            while (true)
            {
                var bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                var log = $"[Client {client.Client.RemoteEndPoint}]: {message}";
                Console.WriteLine(log);
                LogResponse(log);

                Broadcast($"[User]: {message}", client);
            }
        }
        catch
        {
            /* handle errors or logging */
        }
        finally
        {
            lock (_clientLock)
            {
                _connectedClients.Remove(client);
            }

            client.Close();
            Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected.");
        }
    }

    private static void LogResponse(string response)
    {
        File.AppendAllText("./ChatServerExampleLog.txt", response + "\r\n");
    }
}