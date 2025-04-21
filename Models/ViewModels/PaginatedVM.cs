namespace E_CommerceFIdentityScaff.Models.ViewModels
{
    public class PaginatedVM<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

    }
}
