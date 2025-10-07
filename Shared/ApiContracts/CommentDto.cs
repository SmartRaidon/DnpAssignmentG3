namespace ApiContracts;

public class CommentDto
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string Username { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
}