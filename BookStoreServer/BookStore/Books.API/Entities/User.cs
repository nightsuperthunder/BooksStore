namespace Books.API.Entities;

public class User : Entity {
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public byte[] PasswordSalt { get; set; }

    public Role Role { get; set; }
    public virtual ICollection<Book> LikedBooks { get; set; }
}