namespace ApiContracts;

public class CommentDTO
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required int PostId { get; set; }
    public required string Body { get; set; }
}