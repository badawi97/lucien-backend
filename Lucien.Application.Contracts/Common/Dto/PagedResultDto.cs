namespace Lucien.Application.Contracts.Common.Dto
{
    public class PagedResultDto<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new List<T>();

        public PagedResultDto() { }

        public PagedResultDto(int totalCount, List<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}
