using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    
    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    // GET - /Comments/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCommentAsync([FromRoute] int id)
    {
        Comment comment = await _commentRepository.GetSingleAsync(id);
        CommentDto dto = new()
        {
            Id = comment.Id,
            PostId = comment.PostId,
            UserId = comment.UserId,
            Username = comment.Username,
            Content = comment.Content
        };
        return Ok(dto);
    }
    
    // GET - Comments/post/{id}
    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetCommentsOnPostAsync([FromRoute] int postId)
    {
        var comments = await Task.Run(() => _commentRepository.GetMany()
            .Where(c => c.PostId == postId)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.UserId,
                Username = c.Username,
                Content = c.Content
            })
            .ToList());
        return Ok(comments);
    }

    // POST - create /Comments
    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateCommentAsync(CommentDto request)
    {
        Comment comment = new()
        {
            Id = request.Id,
            PostId = request.PostId,
            UserId = request.UserId,
            Username = request.Username,
            Content = request.Content
        };
        
        Comment created = await _commentRepository.AddAsync(comment);
        Console.WriteLine($"Comment created: {created.Id}");
        return Ok(created);
    }
    
    // PUT - update /Comments/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> UpdateCommentAsync([FromRoute] int id, [FromBody] CommentDto request)
    {
        var comment = _commentRepository.GetMany().FirstOrDefault(c => c.Id == id);
        if(comment == null)
            return NotFound("Comment not found");
        comment.Content = request.Content;
        await _commentRepository.UpdateAsync(comment);
        var response = new CommentDto
        {
            Id = comment.Id, // maybe not needed for editing
            PostId = comment.PostId, // maybe not needed for editing
            UserId = comment.UserId, // maybe not needed for editing
            Username = comment.Username,
            Content = comment.Content
        };
        return Ok(new
        {
            message = "Comment successfully updated!",
            comment = response
        });
    }

    // DELETE /Comments/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Comment>> DeleteCommentAsync([FromRoute] int id)
    {
        var comment = _commentRepository.GetMany().FirstOrDefault(c => c.Id == id);
        if(comment == null)
            return NotFound("Comment not found");
        await _commentRepository.DeleteAsync(id);
        return NoContent();
    }
}