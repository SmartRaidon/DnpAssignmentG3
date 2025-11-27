using ApiContracts;
using EfcRepositories;
using EfcRepositories.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        //queryable list of post entities
        //gets all posts from repository
        IQueryable<Post> query = await _postRepository.GetManyAsync();
        //execute query async
        List<Post> posts = await query.ToListAsync();
        //convert to dtos
        List<PostDTO> postDtos = MapPostsToDto(posts);
        return Ok(postDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDTO>> GetSingle(
        [FromRoute] int id,
        [FromQuery] bool includeAuthor = false,
        [FromQuery] bool includeComments = false)
    {
        //start from GetMany, filter by id
        IQueryable<Post> queryForPost = (await _postRepository.GetManyAsync())
            .Where(p => p.Id == id)
            .AsQueryable();
        
        //include author if requested
        if (includeAuthor)
        {
            queryForPost = queryForPost.Include(p => p.User);
        }

        if (includeComments)
        {
            queryForPost = queryForPost.Include(p => p.Comments);
        }
        
        //project to PostDTO (optional with author and comments)
        PostDTO? postDto = await queryForPost.Select(post => new PostDTO()
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,

            Author = includeAuthor
                ? new UserDTO()
                {
                    Id = post.User.Id,
                    Username = post.User.Username
                }
                : null,

            Comments = includeComments
                ? post.Comments.Select(c => new CommentDTO
                {
                    Id = c.Id,
                    Body = c.Body,
                    UserId = c.UserId,
                    PostId = c.PostId
                }).ToList()
                : new List<CommentDTO>()
        }).FirstOrDefaultAsync();//execute sql and gets one row
        
        //404 if not found
        if (postDto is null)
        {
            return NotFound();
        }
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

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdatePostDTO request)
    {
        Post postToUpdate = MapDtoToPost(id, request);

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

    private Post MapDtoToPost(int id, UpdatePostDTO post)
    {
        return new Post()
        {
            Id = id,
            UserId = post.UserId,
            Title = post.Title,
            Body = post.Body
        };
    }

    private List<PostDTO> MapPostsToDto(IEnumerable<Post> posts)
    {
        List<PostDTO> postDtos = new List<PostDTO>();
        foreach (var post in posts)
        {
            PostDTO postDto = MapPostToDto(post);
            postDtos.Add(postDto);
        }

        return postDtos;
    }
}