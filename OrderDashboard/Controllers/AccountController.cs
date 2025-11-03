using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderDashboard.ViewModels.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrderDashboard.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using OrderDashboard.Database.Entities;
    using OrderDashboard.Database.Entities.ENUMs;
    using OrderDashboard.Repositories;
    using OrderDashboard.Services;

    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public AccountController(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("api/account/login")]
        public async Task<IActionResult> LoginApi([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByEmailAsync(model.Username); 

            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            bool isPasswordValid = _passwordService.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Usuário ou senha inválidos." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token = token });
        }

        [HttpGet]
        public IActionResult Register()
        {
            // Apenas exibe a página de registro vazia.
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginViewModel model, [FromQuery] string? ReturnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userRepository.GetUserByEmailAsync(model.Username);

            // Valida o usuário e a senha
            if (user != null && user.IsActive && _passwordService.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                // --- CRIAÇÃO DO COOKIE DE AUTENTICAÇÃO ---
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("name", user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }

                return RedirectToAction("Index", "ServiceOrder");
            }

            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            if (await _userRepository.UserExistsAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                return View(model);
            }

            _passwordService.CreatePasswordHash(model.Password, out string passwordHash, out string passwordSalt);

            var user = new Users
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = UserRole.User, 
                IsActive = false     
            };

            await _userRepository.AddUserAsync(user);

            TempData["SuccessMessage"] = "Registro concluído com sucesso! Faça o login para continuar.";

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        private string GenerateJwtToken (Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("name", user.Name), // Exemplo de claim adicional
            new Claim(ClaimTypes.Role, user.Role.ToString()), // Adicionando a Role
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
