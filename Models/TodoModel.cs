using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapiwc.Models;

public class TodoModel{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime? DeletedAt {get; set;}
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description {get; set;} = string.Empty;
    [Required]
    public bool IsDone {get; set;} = false;
    [Required]
    public string? applicationUserId {get; set;}

    [ForeignKey("applicationUserId")]
    public virtual ApplicationUser? ApplicationUser {get; set;}

    [Required]
    public Guid? UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual UserModel? User {get; set;}
}   