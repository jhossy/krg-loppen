using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Krg.Website.Areas.Admin.Models;

public class UserListReponse
{
    [JsonPropertyName("users")]
    public List<IdentityUser> Users { get; set; }
}