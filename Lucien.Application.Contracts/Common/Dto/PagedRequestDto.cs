namespace Lucien.Application.Contracts.Common.Dto
{
    public class PagedRequestDto
    {
        public int SkipCount { get; set; } = 0;
        public int MaxResultCount { get; set; } = 10;
        public string? Sorting { get; set; }
    }

}
