using Lucien.Application.Contracts.Cards.Dto;
using Lucien.Application.Contracts.Cards.Interfaces;
using Lucien.Application.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lucien.HttpApi.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardApplicationService _applicationService;

        public CardsController(ICardApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IEnumerable<CardDto>> GetLsit()
        {
            return await _applicationService.GetListAsync();
        }

        [HttpGet("{id}")]
        public async Task<CardDto> GetAsync(Guid id)
        {
            return await _applicationService.GetAsync(id);
        }

        [HttpPost]
        public async Task<CardDto> CreateAsync([FromBody] CreateCardDto input)
        {
            return await _applicationService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public async Task<CardDto> UpdateAsync(Guid id, [FromBody] UpdateCardDto input)
        {
            return await _applicationService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _applicationService.DeleteAsync(id);
        }

        [HttpGet("export")]
        public async Task<FileResultDto> ExportAsync([FromQuery] string format = "csv")
        {
            return await _applicationService.ExportAsync(format);
        }

        [HttpPost("import")]
        public CardDto Import(IFormFile file)
        {
            return _applicationService.Import(file);
        }

        [HttpPost("import/qr")]
        public async Task ImportFromQrCodeAsync(IFormFile file)
        {
            await _applicationService.ImportFromQrCodeAsync(file);
        }
    }
}
