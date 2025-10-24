using FileRepositories;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddOpenApi(); // with .net10

builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();

var app = builder.Build();

app.MapControllers();
//app.Environment.EnvironmentName = "Development"; // setting this manually as it caused some issues

// checking environment 
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

// Configure the HTTP request pipeline.
//app.UseOpenApi();       // // with .net10
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); // points to the generated JSON
    c.RoutePrefix = "swagger"; // UI will be available at http://localhost:<port>/swagger
});

app.UseHttpsRedirection();
app.MapGet("/", () => "Web API is running!"); //this line gives a WebApi line on swagger :)
app.Run();