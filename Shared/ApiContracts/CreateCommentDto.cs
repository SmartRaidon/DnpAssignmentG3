namespace ApiContracts;

public class CreateCommentDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public required string Content { get; set; }
}