namespace ApiContracts.DTO.Post;

public class PostCreateDto
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}