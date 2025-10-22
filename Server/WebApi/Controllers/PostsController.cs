using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;


[ApiController]
[Route("[controller]")]

public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetMany()
    {
        IQueryable<Post> posts = await _postRepository.GetManyAsync();

        List<PostDTO> postDtos = MapPostsToDto(posts);

        return Ok(postDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDTO>> GetSingle([FromRoute] int id)
    {
        Post post = await _postRepository.GetSingleAsync(id);

        PostDTO postDto = MapPostToDto(post);

        return Ok(postDto);
    }

    [HttpPost]
    public async Task<ActionResult<PostDTO>> AddPost([FromBody] CreatePostDTO request)
    {
        Post postToAdd = new Post()
        {
            UserId = request.UserId,
            Title = request.Title,
            Body = request.Body
        };

        Post postAdded = await _postRepository.AddAsync(postToAdd);

        PostDTO postToReturn = MapPostToDto(postAdded);
        
        return Ok(postToReturn);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdatePostDTO request)
    {
        Post postToUpdate = MapDtoToPost(request);

        await _postRepository.UpdateAsync(postToUpdate);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await _postRepository.DeleteAsync(id);

        return Ok();
    }

    private PostDTO MapPostToDto(Post post)
    {
        return new PostDTO()
        {
            Id = post.Id,
            UserId = post.UserId,
            Title = post.Title,
            Body = post.Body
        };
    }

    private Post MapDtoToPost(UpdatePostDTO post)
    {
        return new Post()
        {
            Id = post.Id,
            UserId = post.UserId,
            Title = post.Title,
            Body = post.Body
        };
    }

    private List<PostDTO> MapPostsToDto(IQueryable<Post> posts)
    {
        List<PostDTO> postDtos = new List<PostDTO>();
        foreach (var post in posts)
        {
            PostDTO postDto = MapPostToDto(post);
            postDtos.Add(postDto);
        }

        return postDtos;
        {
            
        }
    }
}