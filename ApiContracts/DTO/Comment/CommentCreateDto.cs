namespace ApiContracts.DTO.Comment;

public class CommentCreateDto
{

    public string Username { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
}