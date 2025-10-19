using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlantSearch.Models;

public class EditPlantModel : PageModel
{
    private readonly Tree3Context _context;

    public EditPlantModel(Tree3Context context)
    {
        _context = context;
    }

    [BindProperty]
    public string? NewImageUrl { get; set; }

    [BindProperty]
    public Plant Plant { get; set; } = default!;

    public List<SoilType> SoilTypes { get; set; } = new();
    public List<Season> Seasons { get; set; } = new();
    public List<SuitableRegion> Regions { get; set; } = new();
    public List<PlantType> Categories { get; set; } = new();
    public List<DiseaseType> AllDiseases { get; set; } = new();
    public List<FertilizerType> AllFertilizers { get; set; } = new();
    public List<CultivationMethod> AllMethods { get; set; } = new();
    [BindProperty]
    public List<int> SelectedDiseaseIds { get; set; } = new();

    [BindProperty]
    public List<int> SelectedFertilizerIds { get; set; } = new();

    [BindProperty]
    public List<int> SelectedRegionIds { get; set; } = new();

    [BindProperty]
    public List<int> SelectedSeasonIds { get; set; } = new();

    [BindProperty]
    public List<int> SelectedSoilIds { get; set; } = new();

    [BindProperty]
    public List<int> SelectedTypeIds { get; set; } = new();


    [BindProperty]
    public List<int> SelectedMethodIds { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Plant = await _context.Plants
    .Include(p => p.Soils)
    .Include(p => p.Seasons)
    .Include(p => p.Regions)
    .Include(p => p.Types)
    .Include(p => p.Plantimages)
    .Include(p => p.Methods)
    .Include(p => p.Diseases)
    .Include(p => p.Fertilizers)
    .FirstOrDefaultAsync(p => p.Id == id);

        if (Plant == null)
        {
            return NotFound();
        }

        SoilTypes = await _context.SoilTypes.ToListAsync();
        Seasons = await _context.Seasons.ToListAsync();
        Regions = await _context.SuitableRegions.ToListAsync();
        Categories = await _context.PlantTypes.ToListAsync();
        AllDiseases = await _context.DiseaseTypes.ToListAsync();
        AllFertilizers = await _context.FertilizerTypes.ToListAsync();
        AllMethods = await _context.CultivationMethods.ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Console.WriteLine("📥 Đã vào OnPostAsync");
        try
        {
            var plantInDb = await _context.Plants
       .Include(p => p.Plantimages)
       .Include(p => p.Methods)
       .FirstOrDefaultAsync(p => p.Id == Plant.Id);

            if (plantInDb == null)
            {
                 Console.WriteLine("❌ Không tìm thấy cây trồng với ID: " + Plant.Id);
                return NotFound();
            }

            // Cập nhật các trường cơ bản
            plantInDb.PlantName = Plant.PlantName;
            plantInDb.ScienceName = Plant.ScienceName;
            plantInDb.Origin = Plant.Origin;
            plantInDb.GrowthDuration = Plant.GrowthDuration;
            plantInDb.Description = Plant.Description;
            plantInDb.UpdatedAt = DateTime.Now;

            // Cập nhật ảnh
            if (!string.IsNullOrEmpty(NewImageUrl))
            {
                plantInDb.Plantimages.Clear();
                plantInDb.Plantimages.Add(new Plantimage
                {
                    ImageUrl = NewImageUrl,
                    PlantId = plantInDb.Id
                });
            }

            // Cập nhật phương pháp canh tác
            plantInDb.Methods.Clear();
            var selectedMethods = await _context.CultivationMethods
                .Where(m => SelectedMethodIds.Contains(m.Id))
                .ToListAsync();
            foreach (var method in selectedMethods)
            {
                plantInDb.Methods.Add(method);
            }

            // Cập nhật các liên kết nhiều-nhiều khác
            plantInDb.Diseases = await _context.DiseaseTypes
         .Where(d => SelectedDiseaseIds.Contains(d.Id))
         .ToListAsync();

            plantInDb.Fertilizers = await _context.FertilizerTypes
                .Where(f => SelectedFertilizerIds.Contains(f.Id))
                .ToListAsync();

            plantInDb.Regions = await _context.SuitableRegions
                .Where(r => SelectedRegionIds.Contains(r.Id))
                .ToListAsync();

            plantInDb.Seasons = await _context.Seasons
                .Where(s => SelectedSeasonIds.Contains(s.Id))
                .ToListAsync();

            plantInDb.Soils = await _context.SoilTypes
                .Where(s => SelectedSoilIds.Contains(s.Id))
                .ToListAsync();

            plantInDb.Types = await _context.PlantTypes
                .Where(t => SelectedTypeIds.Contains(t.Id))
                .ToListAsync();

            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Redirecting to /Admin/Plants");
            return RedirectToPage("/Admin/Plants");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Lỗi khi cập nhật: " + ex.Message);
            if (ex.InnerException != null)
    {
        Console.WriteLine("🔍 Chi tiết lỗi: " + ex.InnerException.Message);
    }
            return Page(); // hoặc hiển thị thông báo lỗi
        }
    }
}
