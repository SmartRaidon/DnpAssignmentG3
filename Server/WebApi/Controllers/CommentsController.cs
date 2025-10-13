using ApiContracts.DTO.Comment;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;
[Route("api/posts/{postId}/comments")]
[ApiController]
public class CommentsController: ControllerBase
{
    private readonly ICommentRepository  repository;
    public CommentsController(ICommentRepository repo)
    {
        repository = repo;
    }

    [HttpGet]
    public  IActionResult GetAll([FromRoute] int postId, [FromQuery] string? username,  [FromQuery] int? userId)
    {
        var comments = repository.GetMany().Where(com => com.PostId==postId).ToList();
        if (comments ==null ||comments.Count == 0)
        {
            //throw new Exception("No Comments Found");
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            comments= comments.Where(com =>  com.Username.Contains(username,StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (userId.HasValue)
        {
            comments= comments.Where(com => com.UserId==userId.Value).ToList();
        }

     

        var commentsDTO = comments.Select(comment => new CommentDto()
            {
                Id = comment.Id,
                PostId = comment.PostId,
                Content = comment.Content,
                UserId = comment.UserId,
                Username = comment.Username
            }
        );
        
        return Ok(commentsDTO);
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> GetById([FromRoute] int postId)
    {
        var commentById = await repository.GetSingleAsync(postId);
        if (commentById == null)
        {
            return NotFound();//same as throwing exception like above method
        }

        var commentByIdDto = new CommentDto
        {
            Id = commentById.Id,
            PostId = commentById.PostId,
            Content = commentById.Content,
            UserId = commentById.UserId,
            Username = commentById.Username
        };
        
        return Ok(commentByIdDto);
    }

    [HttpPost]
    public async Task <IActionResult> Create(CommentCreateDto comment, [FromRoute] int postId)
    {
        var commentToCreate = new Comment
        {
            PostId = postId,
            Username = comment.Username,
            Content = comment.Content,
            UserId = comment.UserId
        };
        await repository.AddAsync(commentToCreate);
        var resultOfCreation = new Comment
        {
            Id = commentToCreate.Id,
            PostId = commentToCreate.PostId,
            Content = commentToCreate.Content,
            UserId = commentToCreate.UserId,
            Username = commentToCreate.Username
        };
        return CreatedAtAction(nameof(GetById), new { postId = postId, id = commentToCreate.Id }, resultOfCreation);
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> Update([FromRoute] int postId, CommentUpdateDto comment)
    {
        
        var existingComment = await repository.GetSingleAsync(postId);
        if (existingComment == null)
        {
            return NotFound();
        }
        existingComment.Content = comment.Content;
     
        await repository.UpdateAsync(existingComment);
        return NoContent();
        
    }

    [HttpDelete("{id}")]
    public async Task <IActionResult> Delete([FromRoute] int postId)
    {
        var commentToDelete = await repository.GetSingleAsync(postId);
        if (commentToDelete == null)
        {
            return NotFound();
        }

        await repository.DeleteAsync(postId);
        return NoContent();
    }

}