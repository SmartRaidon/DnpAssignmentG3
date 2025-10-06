using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    
    public CommentController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }
}