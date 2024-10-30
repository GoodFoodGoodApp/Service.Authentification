namespace Service.User.DTO;
public record UserBase
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
public record User : UserBase
{
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
}
