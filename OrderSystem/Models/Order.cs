namespace OrderSystem.Models;

public enum OrderStatus { Progress, Completed, Cancelled }

public class Order
{
    public User User { get; set; }
    public Guid Id { get; set; }
    public List<Item> Items { get; set; }
    public float TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}