namespace ApiContracts;

public class CommentDto
{
    public required int Id { get; set; }
    public required int PostId { get; set; }
    public required string Username { get; set; }
    public required int UserId { get; set; }
    public required string Content { get; set; }
}