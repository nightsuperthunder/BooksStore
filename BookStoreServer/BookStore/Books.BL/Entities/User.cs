namespace Books.BL.Entities;

public class User : Entity {
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public Role Role { get; set; }
    public virtual ICollection<Book> LikedBooks { get; set; }
}