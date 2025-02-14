using System.Text.Json.Serialization;
using webapiwc.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{

    public string FullName { get; set; } = string.Empty;
    public string? DateOfBirth { get; set; }

    [JsonIgnore]
    public ICollection<TodoModel> Todos { get; set; } = new List<TodoModel>();
}
