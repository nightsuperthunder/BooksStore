namespace Books.API.Entities;

public class Book : Entity {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Isbn { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string PreviewImg { get; set; }
    public float Price { get; set; }

    public virtual ICollection<User> UsersLiked { get; set; }
}