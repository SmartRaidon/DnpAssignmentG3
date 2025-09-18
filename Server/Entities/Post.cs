namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    
    public override string ToString()
    {
        return $"User [Id = {Id}, UserId = {UserId}, Title = {Title}, Body = {Body}]";
    }
}