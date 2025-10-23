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
    public  IActionResult GetAll([FromRoute] int postId,  [FromQuery] int? userId)
    {
        var comments = repository.GetMany().Where(com => com.PostId==postId).ToList();
        if (comments ==null ||comments.Count == 0)
        {
            //throw new Exception("No Comments Found");
            return NotFound();
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

            }
        );
        
        return Ok(commentsDTO);
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> GetById([FromRoute] int postId, [FromRoute] int id)
    {
        var commentById = await repository.GetSingleAsync(id);
        if (commentById == null || commentById.PostId != postId)
        {
            return NotFound();//same as throwing exception like above method
        }

        var commentByIdDto = new CommentDto
        {
            Id = commentById.Id,
            PostId = commentById.PostId,
            Content = commentById.Content,
            UserId = commentById.UserId,
   
        };
        
        return Ok(commentByIdDto);
    }

    [HttpPost]
    public async Task <IActionResult> Create(CommentCreateDto comment, [FromRoute] int postId)
    {
        var commentToCreate = new Comment
        {
            PostId = postId,
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
        };
        return CreatedAtAction(nameof(GetById), new { postId = postId, id = commentToCreate.Id }, resultOfCreation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int postId, [FromRoute] int id, CommentUpdateDto comment)
    {
        var existingComment = await repository.GetSingleAsync(id);
        if (existingComment == null || existingComment.PostId != postId)
        {
            return NotFound();
        }

        existingComment.Content = comment.Content;
        await repository.UpdateAsync(existingComment);
        return NoContent();
    }





    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int postId, [FromRoute] int id)
    {
        var commentToDelete = await repository.GetSingleAsync(id);
        if (commentToDelete == null || commentToDelete.PostId != postId)
        {
            return NotFound();
        }

        await repository.DeleteAsync(id);
        return NoContent();
    }


}