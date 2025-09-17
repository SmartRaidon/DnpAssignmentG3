using InMemoryRepositories;
using RepositoryContracts;
using CLI.UI;
using Entities;

// Instantiate repositories
IUserRepository userRepository = new UserInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

// Initialize with dummy data
await InitializeDummyData();

// Start the CLI application
var cliApp = new CliApp(userRepository, postRepository, commentRepository);
await cliApp.StartAsync();

async Task InitializeDummyData()
{
    // Add some initial users
    var user1 = await userRepository.AddAsync(new User { Username = "john_doe", Password = "password123" });
    var user2 = await userRepository.AddAsync(new User { Username = "jane_smith", Password = "secure456" });

    // Add some initial posts
    var post1 = await postRepository.AddAsync(new Post { Title = "First Post", Body = "This is the first post content", UserId = user1.Id });
    var post2 = await postRepository.AddAsync(new Post { Title = "Second Post", Body = "Another interesting post", UserId = user2.Id });

    // Add some comments
    await commentRepository.AddAsync(new Comment { PostId = post1.Id, UserId = user2.Id, Content = "Great post!" });
    await commentRepository.AddAsync(new Comment { PostId = post2.Id, UserId = user1.Id, Content = "Nice work!" });
}