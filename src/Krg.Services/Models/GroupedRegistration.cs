namespace Krg.Services.Models;

public class GroupedRegistration
{
    public string Department { get; set; }

    public Dictionary<string, List<BackofficeRegistrationDto>> Registrations { get; set; }
}