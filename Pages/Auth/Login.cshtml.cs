using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantSearch.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace PlantSearch.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Tree3Context _context;

        public LoginModel(Tree3Context context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string? Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public string ErrorMessage { get; set; }
        public void OnGet()
        {
            ErrorMessage = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var account = _context.Accounts
                .FirstOrDefault(a => a.Username == Input.Username && a.Password == Input.Password);

            if (account == null)
            {
                ErrorMessage = "Tài khoản hoặc mật khẩu không đúng.";
                ModelState.AddModelError(string.Empty, "Đăng nhập thất bại.");
                return Page();
            }

            // Tạo danh sách claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, account.Username),
        new Claim(ClaimTypes.Role, account.Role ?? "user")
    };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            // Đăng nhập bằng cookie
            await HttpContext.SignInAsync("MyCookieAuth", principal);

            // Điều hướng theo role
            var role = account.Role?.ToLower();
            if (role == "admin")
            {
                return RedirectToPage("/Admin/AdminDashBoard");
            }
            else if (role == "user")
            {
                return RedirectToPage("/User/View");
            }
            else
            {
                ErrorMessage = "Tài khoản không có quyền truy cập.";
                ModelState.AddModelError(string.Empty, "Vai trò không hợp lệ.");
                return Page();
            }
        }
    }
}
