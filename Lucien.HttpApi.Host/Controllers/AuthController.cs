using Lucien.Application.Contracts.Auth.Dtos;
using Lucien.Application.Contracts.Auth.Interfaces;
using Lucien.Application.Contracts.Common.Api;
using Lucien.Application.Contracts.Token.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Lucien.HttpApi.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplicationService _applicationService;

        public AuthController(IAuthApplicationService authApplicationService)
        {
            _applicationService = authApplicationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<TokenDto>>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                TokenDto token = await _applicationService.LoginAsync(loginDto);

                SetTokenCookies(token);

                return Ok(new Response<TokenDto>
                {
                    Data = token,
                    Message = "User logged in successfully",
                    Success = true
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new Response<TokenDto>
                {
                    Data = null,
                    Message = "Invalid credentials",
                    Success = false
                });
            }
        }

        [HttpPost("logout")]
        public IActionResult LogoutAsync()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return Ok(new
            {
                message = "Logged out",
                Success = true
            });
        }

        [HttpPost("reset-password")]
        public void ResetPasswordAsync()
        {
        }

        [HttpPost("forgot-password")]
        public void ForgotPasswordAsync()
        {
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<Response<TokenDto>>> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                TokenDto token = await _applicationService.GetRefreshTokenAsync(refreshToken);

                SetTokenCookies(token);

                return Ok(new Response<TokenDto>
                {
                    Data = token,
                    Message = "Token refreshed",
                    Success = true
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new Response<TokenDto>
                {
                    Data = null,
                    Message = "Invalid refresh token",
                    Success = false
                });
            }
        }

        private void SetTokenCookies(TokenDto token)
        {
            var cookieAccessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = token.ExpiresAt
            };

            var cookieRefreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };

            Response.Cookies.Append("accessToken", token.AccessToken ?? "", cookieAccessTokenOptions);
            Response.Cookies.Append("refreshToken", token.RefreshToken ?? "", cookieRefreshTokenOptions);
        }

    }
}
