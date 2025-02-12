using System.ComponentModel.DataAnnotations;

namespace webapiwc.Models;


public class Todo{
    [Key]
    public required Guid Id {get; set;}
    public required DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public required DateTime? DeletedAt {get; set;}
    public required string Title {get; set;}
    public required bool IsDone {get; set;}
    
}