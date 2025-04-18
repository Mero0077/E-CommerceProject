using System.ComponentModel.DataAnnotations;

namespace E_CommerceFIdentityScaff.Models
{
    public enum OrderStatus
    {
        Pending,
        Canceled,
        InProgress,
        Shipped,
        Completed
    }

    public class Order
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }
        public OrderStatus Status { get; set; }
        public bool PaymentStatus { get; set; }
        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentStripeId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
