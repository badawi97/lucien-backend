using Lucien.Application.Contracts.Auth.Dtos;
using Lucien.Application.Contracts.Auth.Interfaces;
using Lucien.Application.Contracts.Common.Dto;
using Lucien.Application.Contracts.Token.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Lucien.HttpApi.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplicationService _authService;

        public AuthController(IAuthApplicationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user and issues tokens.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ResultDto<TokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TokenDto>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResultDto<TokenDto>>> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDto);

                SetTokenCookies(token);

                return Ok(ResultDto<TokenDto>.Success(token, "User logged in successfully"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ResultDto<TokenDto>.Failure("Invalid credentials"));
            }
        }

        /// <summary>
        /// Logs out the user by removing tokens from cookies.
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Ok(new { Message = "Logged out", Success = true });
        }

        /// <summary>
        /// Refreshes access and refresh tokens using the refresh token in cookies.
        /// </summary>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ResultDto<TokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<TokenDto>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResultDto<TokenDto>>> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                    return Unauthorized(ResultDto<TokenDto>.Failure("Refresh token not found"));

                var token = await _authService.GetRefreshTokenAsync(refreshToken);

                SetTokenCookies(token);

                return Ok(ResultDto<TokenDto>.Success(token, "Token refreshed"));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(ResultDto<TokenDto>.Failure("Invalid refresh token"));
            }
        }

        /// <summary>
        /// Initiates password reset process (e.g. sends email).
        /// </summary>
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult ForgotPasswordAsync()
        {
            // TODO: Implement logic (e.g. send email with OTP or reset link)
            return StatusCode(StatusCodes.Status501NotImplemented, new { Message = "Forgot password endpoint not yet implemented." });
        }

        /// <summary>
        /// Resets the password using provided credentials/token.
        /// </summary>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public IActionResult ResetPasswordAsync()
        {
            // TODO: Implement logic (e.g. verify token and change password)
            return StatusCode(StatusCodes.Status501NotImplemented, new { Message = "Reset password endpoint not yet implemented." });
        }

        /// <summary>
        /// Appends secure cookies for access and refresh tokens.
        /// </summary>
        private void SetTokenCookies(TokenDto token)
        {
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.ExpiresAt
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.RefreshTokenExpiresAt 
            };

            Response.Cookies.Append("accessToken", token.AccessToken ?? "", accessTokenOptions);
            Response.Cookies.Append("refreshToken", token.RefreshToken ?? "", refreshTokenOptions);
        }
    }
}
