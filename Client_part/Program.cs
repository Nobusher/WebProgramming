using System.Net.Sockets;
using System.Text;

namespace Client_part
{
    internal class Program
    {
        private const int Port = 5000;
        static async Task Main(string[] args)
        {
            Console.Write("Server IP: ");
            string host = Console.ReadLine()!.Trim();

            using var client = new TcpClient();

            if (!await TryConnectAsync(client, host))
                return;
            Console.WriteLine("Connected. Waiting for messages...");
            await RecieveMesageAsync(client);
            Console.WriteLine("Connection closed");

            
        }
        private static async Task<bool> TryConnectAsync(TcpClient client, string host) 
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            try
            {
                await client.ConnectAsync(host, Port, cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {

                Console.WriteLine("Timeout");
            }
            catch (SocketException ex) 
            {
                Console.WriteLine($"An erroe occurred {ex.SocketErrorCode}");
            }

            return false;
        }
        private static async Task RecieveMesageAsync(TcpClient client) 
        {
            using var reader = new StreamReader(client.GetStream(), Encoding.UTF8);

            try
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null) 
                {
                    Console.WriteLine($"[Server]: {line}");
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Connection error!");
            }
        }
    }
}
