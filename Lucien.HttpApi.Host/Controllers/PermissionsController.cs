using Lucien.Application.Contracts.Common.Dto;
using Lucien.Application.Contracts.Permissions.Interfaces;
using Lucien.Domain.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lucien.HttpApi.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionApplicationService _applicationService;

        public PermissionsController(IPermissionApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Authorize(Policy = PermissionNames.PermissionsRead)]
        [ProducesResponseType(typeof(ResultDto<IReadOnlyCollection<string>>), StatusCodes.Status200OK)]
        public ActionResult<ResultDto<IReadOnlyCollection<string>>> GetListAsync()
        {
            var result = _applicationService.GetCatalog();
            return Ok(ResultDto<IReadOnlyCollection<string>>.Success(result, "Permissions fetched"));
        }
    }
}
