using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using gRPC_demo_B;

namespace gRPC_demo_Main
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:5253");
            var client = new GreeterB.GreeterBClient(channel);
            var reply = await client.SayHelloBAsync(
                              new HelloRequestB { Name = "gRPC_demo_Main" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
