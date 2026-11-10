using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;

namespace WebProgramming
{
    internal class Program
    {
        private const int Port = 5000;
        private static readonly List<StreamWriter> Writers = new List<StreamWriter>();
        static async Task Main(string[] args)
        {
            /* var order = new Order(
                 Id: 1001,
                 Status: OrderStatus.Paid,
                 CreatedAt: new DateTime(2026, 11, 10),
                 Customer: new Customer("Phoenix", "Wright2000@gmail.com"),
                 Items: 
                 [
                     new OrderItem("Laptop", 1, 89990m),
                     new OrderItem("Mouse", 2, 1490m)
                 ]
             );

             var option = new JsonSerializerOptions 
             { WriteIndented = true };
             string json = JsonSerializer.Serialize(order, option);
             File.WriteAllText("user.json",json )
            */
            /*string jsn = File.ReadAllText("user.json");
            Console.WriteLine(jsn);

            var root = JsonNode.Parse(jsn);

            JsonNode? node = root!["Status"];
            Console.WriteLine(node?.GetValueKind());*/
            /*  using var http = new HttpClient();

              var root = await http.GetFromJsonAsync<JsonNode>
                  ("https://jsonplaceholder.typicode.com/users/1");
              Console.WriteLine(root);

              Console.WriteLine(root!["name"]);
              Console.WriteLine(root!["address"]!["city"]);
              Console.WriteLine(root!["company"]!["name"]);*/

            PrintLocalAdresses();

            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine($"\nServer is listening on {Port}. Waiting for client...");

            _ = AcceptCleintsAsync(listener);
            await SendMessagesAsync();

            listener.Stop();

        }
        private static async Task AcceptCleintsAsync(TcpListener listener) 
        {
            while (true) 
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                var writer = new StreamWriter(client.GetStream(), new UTF8Encoding(false))
                {
                    AutoFlush = true
                };

                lock(Writers) Writers.Add(writer);
                Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
            }
        }
        private static void PrintLocalAdresses() 
        {
            Console.WriteLine("IP-Adresses:");
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName())) 
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) 
                {
                    Console.WriteLine($"{ip}");
                }
            }
        }

        private static async Task SendMessagesAsync() 
        {
            

            try
            {
                while (true) 
                {
                    string? line = Console.ReadLine();
                    if (line is null || line == "/exit")
                        break;
                    StreamWriter[] snapshot;
                    lock (Writers)
                    {
                        snapshot = Writers.ToArray();
                    }
                    foreach (StreamWriter writerr in snapshot) 
                    {
                        try
                        {
                            await writerr.WriteLineAsync(line);
                        }
                        catch (IOException)
                        {

                            lock (Writers) Writers.Remove(writerr);
                            Console.WriteLine("Client disconnected");
                        }
                    }
                }
            }
            catch (Exception)
            {

                Console.WriteLine("Client disconnected");
            }
        }

    }
    /* public enum OrderStatus { New, Paid, Shipped }
     public record Customer(string Name, string Email);
     public record OrderItem(string Title, int Quantity, decimal Price);
     public record Order(int Id, OrderStatus Status, DateTime CreatedAt, 
         Customer Customer, List<OrderItem> Items);*/
}
