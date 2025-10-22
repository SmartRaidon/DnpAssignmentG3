using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComment;

public class CreateCommentView
{
    private ICommentRepository _commentRepository; //serve all for comment

    public CreateCommentView(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task
        AddCommentAsync(int userId, int postId,
            string content) //async so to not block the program
    {
        Comment commentToAdd = new Comment
        {
            UserId = userId,
            PostId = postId,
            Body = content
        };

        Comment createdComment =
            await _commentRepository.AddAsync(commentToAdd);
        Console.WriteLine($"Comment created with details: {createdComment}");
    }
}