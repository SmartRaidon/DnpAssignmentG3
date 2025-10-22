namespace ApiContracts;

public class CreatePostDTO
{
    public required int UserId { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
}