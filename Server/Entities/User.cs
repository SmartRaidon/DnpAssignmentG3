namespace Entities;

public class User
{
    private User()
    {
        
    }

    public User(String username, String password)
    {
        Username = username;
        Password = password;
    }
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; }=string.Empty;

    public List<Post> Posts { get; set; } = new List<Post>();
    public List<Comment> Comments { get; set; }= new List<Comment>();
}