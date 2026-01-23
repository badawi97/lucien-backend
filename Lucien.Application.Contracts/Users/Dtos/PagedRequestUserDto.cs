using Lucien.Application.Contracts.Common.Dto;

namespace Lucien.Application.Contracts.Users.Dtos
{
    public class PagedRequestUserDto : PagedRequestDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
    }
}
