namespace Books.API.Entities;

public class Role : Entity {
    public string Name { get; set; }

    public virtual ICollection<User> Users { get; set; }
}