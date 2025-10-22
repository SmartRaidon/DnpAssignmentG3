// See https://aka.ms/new-console-template for more information

using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI App...");

IUserRepository userRepository = new UserFileRepository();
IPostRepository postRepository = new PostFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();

CliApp cliApp = new CliApp(userRepository, postRepository, commentRepository);
await cliApp.startAsync();