using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniYemekSepeti.Services;
using MiniYemekSepeti.Models;

namespace MiniYemekSepeti.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Siparişin toplam tutarını döndüren fonksiyon
        public async Task<decimal> GetTotalAmountAsync(int orderId)
        {
            var order = await _context.Orders.Include(o => o.Items)
                                              .ThenInclude(oi => oi.Food)
                                              .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return 0;

            decimal totalAmount = 0;

            // Siparişteki her bir öğenin toplam tutarını hesapla
            foreach (var item in order.Items)
            {
                totalAmount += item.Food.Price * item.Quantity;
            }

            return totalAmount;
        }

        // Tüm siparişlerin toplam tutarını döndüren fonksiyon
        public async Task<decimal> GetTotalAmountForAllOrdersAsync()
        {
            var orders = await _context.Orders.Include(o => o.Items)
                                              .ThenInclude(oi => oi.Food)
                                              .ToListAsync();

            decimal totalAmount = 0;

            foreach (var order in orders)
            {
                foreach (var item in order.Items)
                {
                    totalAmount += item.Food.Price * item.Quantity;
                }
            }

            return totalAmount;
        }



    }
}
