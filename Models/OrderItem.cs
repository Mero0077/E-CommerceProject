﻿using Microsoft.EntityFrameworkCore;

namespace E_CommerceFIdentityScaff.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderItem
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }
    }
}
