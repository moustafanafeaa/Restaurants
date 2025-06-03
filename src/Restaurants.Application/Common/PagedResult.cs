namespace Restaurants.Application.Common
{
    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> items,int totalCount,int pageSize = 5, int pageNumber = 1)
        {
            Items = items;
            TotalItemsCount = totalCount;
            TotalPages =(int)Math.Ceiling(totalCount / (double)pageSize);  // => 14 / 5 = 3 
            ItemFrom = pageSize * (pageNumber - 1) + 1;
            ItemTo = ItemFrom + pageSize - 1;
        }
        public IEnumerable<T> Items { get; set; }
        public int TotalItemsCount { get; set; }
        public int TotalPages { get; set; } 
        public int ItemFrom { get; set; }
        public int ItemTo { get; set; }

    }
}
