namespace EfcRepositories;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Post> UserPosts { get; set; }
    public List<Comment> UserComments { get; set; }
}