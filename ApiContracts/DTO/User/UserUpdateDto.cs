namespace ApiContracts.DTO.User;

public class UserUpdateDto
{
    public required string Username { get; set; }
    public string? Password { get; set; }

}