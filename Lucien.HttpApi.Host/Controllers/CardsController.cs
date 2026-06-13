using Lucien.Application.Contracts.Cards.Dto;
using Lucien.Application.Contracts.Cards.Interfaces;
using Lucien.Application.Contracts.Common.Dto;
using Lucien.Domain.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucien.HttpApi.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly ICardApplicationService _applicationService;

        public CardsController(ICardApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Authorize(Policy = PermissionNames.CardsRead)]
        public async Task<IEnumerable<CardDto>> GetLsit()
        {
            return await _applicationService.GetListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionNames.CardsRead)]
        public async Task<CardDto> GetAsync(Guid id)
        {
            return await _applicationService.GetAsync(id);
        }

        [HttpPost]
        [Authorize(Policy = PermissionNames.CardsCreate)]
        public async Task<CardDto> CreateAsync([FromBody] CreateCardDto input)
        {
            return await _applicationService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionNames.CardsUpdate)]
        public async Task<CardDto> UpdateAsync(Guid id, [FromBody] UpdateCardDto input)
        {
            return await _applicationService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionNames.CardsDelete)]
        public async Task DeleteAsync(Guid id)
        {
            await _applicationService.DeleteAsync(id);
        }

        [HttpGet("export")]
        [Authorize(Policy = PermissionNames.CardsExport)]
        public async Task<FileResultDto> ExportAsync([FromQuery] string format = "csv")
        {
            return await _applicationService.ExportAsync(format);
        }

        [HttpPost("import")]
        [Authorize(Policy = PermissionNames.CardsImport)]
        public CardDto Import(IFormFile file)
        {
            return _applicationService.Import(file);
        }

        [HttpPost("import/qr")]
        [Authorize(Policy = PermissionNames.CardsImport)]
        public async Task ImportFromQrCodeAsync(IFormFile file)
        {
            await _applicationService.ImportFromQrCodeAsync(file);
        }
    }
}
