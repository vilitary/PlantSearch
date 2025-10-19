using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlantSearch.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PlantSearch.Pages
{
    public class DetailModel : PageModel
    {
        private readonly Tree3Context _context;

        public DetailModel(Tree3Context context)
        {
            _context = context;
        }

        public Plant? Plant { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<Diseasetreatment> DiseaseTreatments { get; set; } = new();
        public List<CultivationMethod> CultivationMethods { get; set; } = new();
        public List<SoilType> Soils { get; set; } = new();
        public List<FertilizerType> Fertilizers { get; set; } = new();

        // ✅ Thêm mới:
        public List<Comment> Comments { get; set; } = new();
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Plant = await _context.Plants
                .Include(p => p.Types)
                .Include(p => p.Seasons)
                .Include(p => p.Regions)
                .Include(p => p.Soils)
                .Include(p => p.Fertilizers)
                .Include(p => p.Methods)
                .Include(p => p.Diseases)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Plant == null)
                return NotFound();

            ImageUrls = await _context.Plantimages
                .Where(img => img.PlantId == id)
                .Select(img => img.ImageUrl)
                .ToListAsync();

            // ✅ Bệnh & điều trị
            var diseaseIds = Plant.Diseases.Select(d => d.Id).ToList();

            DiseaseTreatments = await _context.Diseasetreatments
                .Include(dt => dt.Disease)
                .Where(dt => dt.DiseaseId != null && diseaseIds.Contains(dt.DiseaseId.Value))
                .ToListAsync();

            CultivationMethods = Plant.Methods.ToList();
            Soils = Plant.Soils.ToList();
            Fertilizers = Plant.Fertilizers.ToList();

            // ✅ Lấy bình luận
            Comments = await _context.Comments
                .Include(c => c.Account)
                .Where(c => c.PlantId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            // ✅ Lấy đánh giá trung bình
            var ratings = await _context.Ratings
                .Where(r => r.PlantId == id)
                .ToListAsync();

            if (ratings.Any())
            {
                AverageRating = ratings.Average(r => r.RatingValue);
                RatingCount = ratings.Count;
            }
            else
            {
                AverageRating = 0;
                RatingCount = 0;
            }

            return Page();
        }
        [BindProperty]
        public string Content { get; set; }

        [BindProperty]
        public int PlantId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToPage("/Auth/Login");
            }

            if (string.IsNullOrWhiteSpace(Content))
            {
                TempData["Error"] = "Bình luận không được để trống!";
                return RedirectToPage(new { id = PlantId });
            }

            // Lấy user hiện tại
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return RedirectToPage("/Auth/Login");

            // Lấy đối tượng Account từ database
            var account = await _context.Accounts.FindAsync(userId); // nếu key là string
            if (account == null)
                return RedirectToPage("/Account/Login");

            var comment = new Comment
            {
                PlantId = PlantId,
                Account = account,  // ✅ gán đối tượng Account
                Content = Content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id = PlantId });
        }
    }
}
