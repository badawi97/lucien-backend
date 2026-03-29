using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucien.Application.Contracts.Auth.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!; 
        public string? Role { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
