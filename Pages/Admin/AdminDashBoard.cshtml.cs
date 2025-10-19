using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlantSearch.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantSearch.Pages.Admin
{
    public class AdminDashBoardModel : PageModel
    {
        private readonly Tree3Context _context;

        public AdminDashBoardModel(Tree3Context context)
        {
            _context = context;
        }

        // Thống kê chung
        public int TotalPlants { get; set; }
        public int TotalPlantTypes { get; set; }
        public int TotalAdmins { get; set; }

        // Biểu đồ cây theo tháng
        public List<string> MonthLabels { get; set; } = new();
        public List<int> MonthlyPlantCounts { get; set; } = new();

        // Top 3 cây theo rating
        public List<PlantRatingViewModel> TopPlants { get; set; } = new();

        public class PlantRatingViewModel
        {
            public string PlantName { get; set; }
            public double AverageRating { get; set; }
            public int RatingCount { get; set; }
        }

        public async Task OnGetAsync()
        {
            // Tổng số cây, loại cây, admin
            TotalPlants = await _context.Plants.CountAsync();
            TotalPlantTypes = await _context.PlantTypes.CountAsync();
            TotalAdmins = await _context.Accounts
                .Where(a => a.Role.ToLower() == "admin")
                .CountAsync();

            // Biểu đồ cây theo tháng
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            for (int month = 1; month <= currentMonth; month++)
            {
                MonthLabels.Add($"Tháng {month}");

                var count = await _context.Plants
                    .Where(p => p.CreatedAt.HasValue &&
                                p.CreatedAt.Value.Month == month &&
                                p.CreatedAt.Value.Year == currentYear)
                    .CountAsync();

                MonthlyPlantCounts.Add(count);
            }

            // Lấy top 3 cây theo rating (rating là bảng riêng)
            TopPlants = await _context.Plants
                .Select(p => new PlantRatingViewModel
                {
                    PlantName = p.PlantName,
                    AverageRating = _context.Ratings
                        .Where(r => r.PlantId == p.Id)
                        .Select(r => (double?)r.RatingValue)
                        .Average() ?? 0,
                    RatingCount = _context.Ratings.Count(r => r.PlantId == p.Id)
                })
                .OrderByDescending(p => p.AverageRating)
                .ThenByDescending(p => p.RatingCount)
                .Take(3)
                .ToListAsync();
        }
    }
}
