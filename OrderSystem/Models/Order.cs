namespace OrderSystem.Models;

public class Order
{
    public User User { get; set; }
    public Guid Id { get; set; }
    public List<Item> Items { get; set; }
}