using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using webapiwc.Models;

public class UserModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [JsonIgnore] // Prevents exposing hashed password in API responses
    public string PasswordHash { get; set; } = string.Empty;

    [JsonIgnore]
    public virtual ICollection<TodoModel> Todos { get; set; } = new List<TodoModel>();
}
