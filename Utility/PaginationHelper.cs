using E_CommerceFIdentityScaff.Models.ViewModels;

namespace E_CommerceFIdentityScaff.Utility
{
    public class PaginationHelper

    {
        public static PaginatedVM<T> Paginate<T>(List<T> source, int page, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedVM<T>
            {
                Items = items,
                PageNumber = page,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
        }
    }
}
