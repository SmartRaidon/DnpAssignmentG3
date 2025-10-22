namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Body { get; set; }
    
    public override string ToString()
    {
        return $"Comment [Id = {Id}, UserId = {UserId}, PostId = {PostId}, Body = {Body}]";
    }
}