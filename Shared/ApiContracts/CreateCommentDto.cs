namespace ApiContracts;

public class CreateCommentDto
{
    public int PostId { get; set; }
    public string Username { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
}