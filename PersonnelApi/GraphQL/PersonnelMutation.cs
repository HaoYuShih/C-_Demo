using PersonnelApi.Models;

namespace PersonnelApi.GraphQL;

public class PersonnelMutation
{
    public record SubmitPersonInput(string Name, string Email, int Age);
    public record SubmitPersonPayload(string Message);

    public SubmitPersonPayload SubmitPerson(SubmitPersonInput input, [Service] ILogger<PersonnelMutation> logger)
    {
        logger.LogInformation("GraphQL mutation received person: {Name}, {Email}, {Age}", input.Name, input.Email, input.Age);

        // In a real application, you would save this to a database.
        // For this demo, we just return a success response.

        var person = new Person
        {
            Name = input.Name,
            Email = input.Email,
            Age = input.Age
        };

        return new SubmitPersonPayload($"Received person {person.Name} via GraphQL.");
    }
}
