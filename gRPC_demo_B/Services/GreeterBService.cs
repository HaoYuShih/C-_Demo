using Grpc.Core;
using Grpc.Net.Client;
using gRPC_demo_A;
using gRPC_demo_B;

namespace gRPC_demo_B.Services
{
    public class GreeterBService : GreeterB.GreeterBBase
    {
        private readonly ILogger<GreeterBService> _logger;
        private readonly IConfiguration _configuration;

        public GreeterBService(ILogger<GreeterBService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<HelloReplyB> SayHelloB(HelloRequestB request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC_demo_B received a request: {Name}", request.Name);

            var serviceAUrl = _configuration["ServiceAUrl"];
            _logger.LogInformation("Connecting to gRPC_demo_A at {Url}", serviceAUrl);

            if (string.IsNullOrEmpty(serviceAUrl))
            {
                _logger.LogError("ServiceAUrl is not configured.");
                throw new RpcException(new Status(StatusCode.Internal, "ServiceAUrl is not configured."));
            }

            using var channel = GrpcChannel.ForAddress(serviceAUrl);
            var client = new GreeterA.GreeterAClient(channel);

            var replyFromA = await client.SayHelloAAsync(
                              new HelloRequestA { Name = request.Name });

            _logger.LogInformation("gRPC_demo_A replied: {Message}", replyFromA.Message);

            return new HelloReplyB
            {
                Message = "gRPC_demo_B received a message from gRPC_demo_A: '" + replyFromA.Message + "'"
            };
        }
    }
}
