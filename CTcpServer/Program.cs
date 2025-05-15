namespace CTcpServer;

public class Program
{
    static void Main()
    {
        // CTcpServer.Run();

        var server = new ChatServerExample();
        server.Run();
    }
}