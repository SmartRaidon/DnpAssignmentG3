using CLI.UI.ManageComment;
using CLI.UI.ManagePost;
using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    //repository specific
    private IUserRepository _userRepository;
    private IPostRepository _postRepository;
    private ICommentRepository _commentRepository;

    // user view specific
    private CreateUserView _createUserView;
    private SingleUsersView _singleUsersView;
    private ListUsersView _listUsersView;

    // post view specific
    private CreatePostView _createPostView;
    private UpdatePostView _updatePostView;
    private SinglePostView _singlePostView;
    private ListPostsView _listPostsView;
    
    // comment view specific
    private CreateCommentView _createCommentView;


    public CliApp(IUserRepository userRepository,
        IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;

        _createUserView = new CreateUserView(userRepository);
        _singleUsersView = new SingleUsersView(userRepository);
        _listUsersView = new ListUsersView(userRepository);

        _createPostView = new CreatePostView(postRepository);
        _updatePostView = new UpdatePostView(postRepository);
        _singlePostView = new SinglePostView(postRepository);
        _listPostsView = new ListPostsView(postRepository);
        
        _createCommentView = new CreateCommentView(commentRepository);
    }

    public async Task startAsync()
    {
        Console.WriteLine("Welcome to the CLI Application");

        bool exit = false;
        while (exit is not true)
        {
            Console.WriteLine("");
            Console.WriteLine("You have the following options: ");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Manage Comments");
            Console.WriteLine("4. Exit application");

            Console.WriteLine("Enter your choice(1-4):");
            int chosenView = ReadNumber();


            switch (chosenView)
            {
                case 1:
                {
                    Console.WriteLine($"You have chosen to Manage users");
                    Console.WriteLine($"Choose one of the following options:");
                    Console.WriteLine($"1. Create user");
                    Console.WriteLine($"2. View Single user");
                    Console.WriteLine($"3. View All users");
                    Console.WriteLine($"4. Go back to main menu");
                    int chosenOption = ReadNumber();

                    switch (chosenOption)
                    {
                        case 1:
                        {
                            string username, password = "";

                            Console.WriteLine("Enter username: ");
                            username = Console.ReadLine();

                            Console.WriteLine("Enter password: ");
                            password = Console.ReadLine();

                            await _createUserView.AddUserAsync(username,
                                password);

                            break;
                        }
                        case 2:
                        {
                            Console.WriteLine("Enter Id of user to view: ");
                            int idUserToView = ReadNumber();

                            await _singleUsersView.GetSingleAsync(idUserToView);
                            break;
                        }
                        case 3:
                        {
                            await _listUsersView.GetManyAsync();
                            break;
                        }
                        case 4:
                        {
                            Console.WriteLine("Going back to main menu");
                            break;
                        }
                        case 5:
                        {
                            Console.WriteLine(
                                "You have chosen the wrong option, try again1");
                            break;
                        }
                        default:
                        {
                            Console.WriteLine(
                                "You have chosen the wrong option, try again");
                            break;
                        }
                    }

                    break;
                }
                case 2:
                {
                    Console.WriteLine($"You have chosen to Manage posts");
                    Console.WriteLine($"Choose one of the following options:");
                    Console.WriteLine($"1. Create post");
                    Console.WriteLine($"2. Update post");
                    Console.WriteLine($"3. View Single post");
                    Console.WriteLine($"4. View All posts");
                    Console.WriteLine($"5. Go back to main menu");
                    
                    int chosenOption = ReadNumber();

                    switch (chosenOption)
                    {
                        case 1:
                        {
                            Console.WriteLine("Enter Id of user to post: ");
                            int idUserToPost = ReadNumber();
                            
                            string title, body = "";
                            Console.WriteLine("Enter title: ");
                            title = Console.ReadLine();
                            if (title is "")
                            {
                                Console.WriteLine(
                                    "Title cannot be empty. Please try again.");
                                break;
                            }

                            Console.WriteLine("Enter body: ");
                            body = Console.ReadLine();
                            if (body is "")
                            {
                                Console.WriteLine(
                                    "Body cannot be empty. Please try again.");
                                break;
                            }

                            await _createPostView.AddPostAsync(idUserToPost, title, body);

                            break;
                        }
                        case 2:
                        {
                            
                            Console.WriteLine("Enter Id of user to update: ");
                            int idUserToUpdate = ReadNumber();
                            await _singleUsersView.GetSingleAsync(
                                idUserToUpdate);
                            
                            Console.WriteLine("Enter Id of post to update: ");
                            int idPostToUpdate = ReadNumber();

                            string title, body = "";

                            Console.WriteLine("Enter title: ");
                            title = Console.ReadLine();

                            if (title is "")
                                title = "No title";

                            Console.WriteLine("Enter body: ");
                            body = Console.ReadLine();

                            await _updatePostView.UpdatePostAsync(idPostToUpdate, new Post()
                            {
                               UserId = idUserToUpdate, Title = title, Body = body
                            });
                            break;
                        }
                        case 3:
                        {
                            Console.WriteLine("Enter Id of post to view: ");
                            int idPostToView = ReadNumber();
                            
                            await _singlePostView.GetSingleAsync(idPostToView);
                            break;
                        }
                        case 4:
                        {
                            await _listPostsView.getManyAsync();
                            break;
                        }
                        case 5:
                        {
                            Console.WriteLine("Going back to main menu");
                            break;
                        }
                        default:
                        {
                            Console.WriteLine(
                                "You have chosen the wrong option, try again");
                            break;
                        }
                    }

                    break;
                }
                case 3:
                {
                    Console.WriteLine($"You have chosen to Manage comments");
                    Console.WriteLine($"Choose one of the following options:");
                    Console.WriteLine($"1. Create comment");
                    Console.WriteLine($"2. Go back to main menu");
                    int chosenOption = ReadNumber();

                    switch (chosenOption)
                    {
                        case 1:
                        {
                            Console.WriteLine("Enter Id of user to comment: ");
                            int idUserToComment = ReadNumber();
                            
                            Console.WriteLine("Enter Id of post to comment: ");
                            int idPostToComment = ReadNumber();
                            
                            string body = "";
                            Console.WriteLine("Enter body: ");
                            body = Console.ReadLine();
                            if (body is "")
                            {
                                Console.WriteLine(
                                    "Body cannot be empty. Please try again.");
                                break;
                            }
                            
                            await _createCommentView.AddCommentAsync(idUserToComment, idPostToComment, body);

                            break;
                        }
                        case 2:
                        {
                            Console.WriteLine("Going back to main menu");
                            break;
                        }
                        default:
                        {
                            Console.WriteLine(
                                "You have chosen the wrong option, try again");
                            break;
                        }
                    }

                    break;
                }
            }
        }

        await Task.CompletedTask;
    }
    
    private int ReadNumber()
    {
        string chosenViewInput = Console.ReadLine();
        if (chosenViewInput is not null || chosenViewInput != "")
        {
            return int.Parse(chosenViewInput);
        }

        return 0;
    }
}