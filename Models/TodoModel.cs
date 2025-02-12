using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapiwc.Models;

public class TodoModel{
    [Key]
    public required Guid Id {get; set;}
    public required DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public required DateTime? DeletedAt {get; set;}
    public required string Title {get; set;}
    public required string Description {get; set;}
    public required bool IsDone {get; set;}

    public Guid? UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual UserModel? User {get; set;}
}   