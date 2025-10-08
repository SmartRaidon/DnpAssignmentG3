using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    
    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    // GET - /Posts/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostAsync([FromRoute] int id)
    {
        Post post = await _postRepository.GetSingleAsync(id);
        PostDto dto = new()
        {
            Id = post.Id,
            Body = post.Body,
            Title = post.Title,
            UserId = post.UserId
        };
        return Ok(dto);
    }

    // GET - /Posts
    [HttpGet]
    public async Task<IActionResult> GetPostsAsync()
    {
        var posts = await Task.Run(() => _postRepository.GetMany()
            .Select(p => new PostDto
            {
                Id = p.Id,
                Body = p.Body,
                Title = p.Title,
                UserId = p.UserId
            }).ToList());
        return Ok(posts);
    }

    // POST - create /Posts
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePostAsync([FromBody] PostDto request)
    {
        Post post = new()
        {
            Body = request.Body,
            Title = request.Title,
            UserId = request.UserId
        };
        
        Post created = await _postRepository.AddAsync(post);
        Console.WriteLine($"Post created: {created.Id}");
        return Ok(created);
    }

    // PUT - update /Posts/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PostDto>> UpdatePostAsync([FromRoute] int id, [FromBody] PostDto request)
    {
        var post = _postRepository.GetMany().FirstOrDefault(p => p.Id == id);
        if (post == null)
            return NotFound("Post not found");
        post.Body = request.Body;
        post.Title = request.Title;
        await _postRepository.UpdateAsync(post);
        var response = new PostDto
        {
            Id = post.Id, // maybe not needed for editing
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId // maybe not needed for editing
        };
        return Ok(new
        {
            message = "Post successfully updated!",
            post = response
        });
    }
    
    // DELETE - /Posts/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Post>> DeletePostAsync([FromRoute] int id)
    {
        var post = _postRepository.GetMany().FirstOrDefault(p => p.Id == id);
        if (post == null)
            return NotFound("Post not found");
        await _postRepository.DeleteAsync(id);
        return NoContent();
    }
}