using Grpc.Core;
using gRPC_demo_A;

namespace gRPC_demo_A.Services
{
    public class GreeterAService : GreeterA.GreeterABase
    {
        private readonly ILogger<GreeterAService> _logger;
        public GreeterAService(ILogger<GreeterAService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReplyA> SayHelloA(HelloRequestA request, ServerCallContext context)
        {
            _logger.LogInformation("gRPC_demo_A received a request: {Name}", request.Name);
            return Task.FromResult(new HelloReplyA
            {
                Message = "Hello from gRPC_demo_A, " + request.Name
            });
        }
    }
}
