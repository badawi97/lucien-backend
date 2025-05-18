using Lucien.Application.Contracts.Cards.Dto;
using Lucien.Application.Contracts.Common;
using Lucien.Domain.Shared.DI;
using Microsoft.AspNetCore.Http;

namespace Lucien.Application.Contracts.Cards.Interfaces
{
    public interface ICardApplicationService : ITransient
    {
        Task<CardDto> GetAsync(Guid id);
        Task<List<CardDto>> GetListAsync();
        Task<CardDto> CreateAsync(CreateCardDto input);
        Task<CardDto> UpdateAsync(Guid id, UpdateCardDto input);
        Task DeleteAsync(Guid id);
        Task<FileResultDto> ExportAsync(string format);
        CardDto Import(IFormFile file);
        Task<CardDto> ImportFromQrCodeAsync(IFormFile file);
    }
}
