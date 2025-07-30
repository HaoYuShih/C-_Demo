using Grpc.Core;
using PersonnelApi.Protos;

namespace PersonnelApi.Services;

public class GrpcPersonnelService : Personnel.PersonnelBase
{
    private readonly ILogger<GrpcPersonnelService> _logger;

    public GrpcPersonnelService(ILogger<GrpcPersonnelService> logger)
    {
        _logger = logger;
    }

    public override Task<PersonReply> SubmitPerson(PersonRequest request, ServerCallContext context)
    {
        _logger.LogInformation("gRPC service received person: {Name}, {Email}, {Age}", request.Name, request.Email, request.Age);

        // In a real application, you would save this to a database.
        // For this demo, we just return a success response.

        return Task.FromResult(new PersonReply
        {
            Message = $"Received person {request.Name} via gRPC."
        });
    }
}
