namespace ApiContracts.DTO.User;

public class UserCreateDto
{
    public required string Username { get; set; }
    public string Password { get; set; } = string.Empty;  // Remove 'required' and provide default
}