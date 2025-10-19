using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace PlantSearch.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Xóa session hoặc cookie nếu có
            HttpContext.SignOutAsync(); // Nếu bạn dùng xác thực cookie

            // Hoặc xóa thủ công nếu bạn dùng session
            HttpContext.Session.Clear();

            // Chuyển hướng về trang đăng nhập
            return RedirectToPage("/User/View");
        }
    }
}
