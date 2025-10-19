using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantSearch.Models;
using Microsoft.EntityFrameworkCore;

public class PlantsModel : PageModel
{
    private readonly Tree3Context _context;

    public PlantsModel(Tree3Context context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchName { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;
    public int TotalPages { get; set; }

    public List<PlantViewModel> Plants { get; set; } = new();
    public List<SoilType> SoilTypes { get; set; } = new();
    public List<Season> Seasons { get; set; } = new();
    public List<SuitableRegion> Regions { get; set; } = new();
    public List<PlantType> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int? SoilTypeId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SeasonId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? RegionId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? CategoriesId { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? CreatedDate { get; set; }

    public async Task OnGetAsync()
    {
        SoilTypes = await _context.SoilTypes.ToListAsync();
        Seasons = await _context.Seasons.ToListAsync();
        Regions = await _context.SuitableRegions.ToListAsync();
        Categories = await _context.PlantTypes.ToListAsync();

        var query = _context.Plants
            .Include(p => p.Plantimages)
            .Include(p => p.Soils)
            .Include(p => p.Seasons)
            .Include(p => p.Regions)
            .Include(p => p.Types)
            .AsQueryable();

        if (!string.IsNullOrEmpty(SearchName))
        {
            query = query.Where(p => p.PlantName.ToLower().Contains(SearchName.ToLower()));
        }

        if (SoilTypeId.HasValue)
        {
            query = query.Where(p => p.Soils.Any(s => s.Id == SoilTypeId));
        }

        if (SeasonId.HasValue)
        {
            query = query.Where(p => p.Seasons.Any(s => s.Id == SeasonId));
        }

        if (RegionId.HasValue)
        {
            query = query.Where(p => p.Regions.Any(r => r.Id == RegionId));
        }

        if (CategoriesId.HasValue)
        {
            query = query.Where(p => p.Types.Any(t => t.Id == CategoriesId));
        }

        if (CreatedDate.HasValue)
        {
            query = query.Where(p => p.CreatedAt.HasValue &&
                                     p.CreatedAt.Value.Date == CreatedDate.Value.Date);
        }

        int totalCount = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        Plants = await query
            .OrderBy(p => p.PlantName)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .Select(p => new PlantViewModel
            {
                Id = p.Id,
                PlantName = p.PlantName,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                ImageUrl = p.Plantimages.FirstOrDefault().ImageUrl
            })
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var plant = await _context.Plants.FindAsync(id);
        if (plant != null)
        {
            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
