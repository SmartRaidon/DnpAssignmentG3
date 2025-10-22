using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]

public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentDTO>>> GetMany()
    {
        IQueryable<Comment> comments = await _commentRepository.GetManyAsync();

        List<CommentDTO> commentDtos = MapCommentsToDto(comments);
        
        return Ok(commentDtos);
    }

    // GET by Id action
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetSingle([FromRoute] int id)
    {
        Comment comment = await _commentRepository.GetSingleAsync(id);

        CommentDTO commentDto = MapCommentToDto(comment);
        
        return Ok(commentDto);
    }
    
    [HttpPost]
    public async Task<ActionResult<CommentDTO>> AddComment([FromBody] CreateCommentDTO request)
    {
        Comment commentToAdd = new Comment()
        {
            UserId = request.UserId,
            PostId = request.PostId,
            Body = request.Body
        };
        Comment commentAdded = await _commentRepository.AddAsync(commentToAdd);

        CommentDTO userToReturn = MapCommentToDto(commentAdded);
        
        return Ok(userToReturn);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCommentDTO request)
    {
        Comment commentToUpdate = MapDtoToComment(request);
        
        await _commentRepository.UpdateAsync(commentToUpdate);

        return Ok();
    }
    
    // DELETE action
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _commentRepository.DeleteAsync(id);

        return Ok();
    }

    private CommentDTO MapCommentToDto(Comment comment)
    {
        return new CommentDTO()
        {
            Id = comment.Id,
            UserId = comment.UserId,
            PostId = comment.PostId,
            Body = comment.Body
        };
    }
    
    private Comment MapDtoToComment(UpdateCommentDTO comment)
    {
        return new Comment()
        {
            Id = comment.Id,
            UserId = comment.UserId,
            PostId = comment.PostId,
            Body = comment.Body
        };
    }


    private List<CommentDTO> MapCommentsToDto(IQueryable<Comment> comments)
    {
        List<CommentDTO> commentDtos = new List<CommentDTO>();
        foreach (var comment in comments)
        {
            CommentDTO commentDto = MapCommentToDto(comment);
            commentDtos.Add(commentDto);
        }

        return commentDtos;
    }
}