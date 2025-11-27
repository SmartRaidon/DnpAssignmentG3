namespace ApiContracts;

public class PostDTO
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    
    public UserDTO? Author { get; set; } //optional author
    public List<CommentDTO> Comments { get; set; } = new(); //optional comments
}