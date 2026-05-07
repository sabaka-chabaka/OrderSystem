using OrderSystem.Data;
using OrderSystem.Models;

namespace OrderSystem.Services;

public class OrderService (IConfiguration config, AppDbContext db)
{
    public async Task<Order?> GetOrderByIdAsync(Guid id)
    {
        return await db.Orders.FindAsync(id);
    }
}