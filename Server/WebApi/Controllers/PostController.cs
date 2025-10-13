using ApiContracts.DTO.Post;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;
[Route("api/posts")]
[ApiController]
public class PostController: ControllerBase
{
    private readonly IPostRepository repo;

    public PostController(IPostRepository postRepository)
    {
        repo=postRepository;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string? title, [FromQuery] int? userId)
    {
        var posts =  repo.GetMany().ToList();
        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts
                .Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (userId.HasValue)
        {
            posts = posts
                .Where(p => p.UserId == userId.Value)
                .ToList();
        }
        
        if (posts.Count ==0 || posts == null)
        {
            return NotFound();
        }
        
        var postDTO = posts.Select(
            post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
            }
            );
        return Ok(postDTO);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var post = await repo.GetSingleAsync(id);
        if (post == null )
        {
            return NotFound();
        }

        var postDTO = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
        };
        return Ok(postDTO);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostCreateDto post)
    {
        var posttocreate = new Post
        {
  
            Title = post.Title,
            Body = post.Body,
            UserId= post.UserId
        };
        
        await repo.AddAsync(posttocreate);
        var resultofcreate = new PostDto
        {
            Id = posttocreate.Id,
            Title = posttocreate.Title,
            Body = posttocreate.Body,
            UserId = posttocreate.UserId,
        };
        return CreatedAtAction(nameof(GetById), new { id = posttocreate.Id }, resultofcreate);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult>  Update([FromRoute] int id, PostUpdateDto postDTO)
    {
       
        var toUpdate = await repo.GetSingleAsync(id);
        if (toUpdate == null )
        {
            return NotFound();
        }
        toUpdate.Title = postDTO.Title;
        toUpdate.Body = postDTO.Body;
       
        await repo.UpdateAsync(toUpdate);
        return NoContent();


    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>  Delete([FromRoute] int id)
    {
        var toDelete = await repo.GetSingleAsync(id);
        if (toDelete is null)
        {
            return NotFound();
        }
        await repo.DeleteAsync(id);
        return NoContent();
        
    }


}