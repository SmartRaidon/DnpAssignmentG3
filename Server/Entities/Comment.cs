namespace Entities;

public class Comment
{
    private Comment() { }
 public Comment(int postId, int userId, string content)
    {
        PostId = postId;
        UserId = userId;
        Content = content;
    }
    public int Id { get; set; }
    public int PostId { get; set; }
    public Post Posts { get; set; }
    public string Username { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public string Content { get; set; }
    
   
}