using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantSearch.Models;
using Microsoft.EntityFrameworkCore;


namespace PlantSearch.Pages;

public class ViewModel : PageModel
{
    
 private readonly Tree3Context _context;

    public ViewModel(Tree3Context context)
    {
        _context = context;
    }

    public List<PlantType> PlantTypes { get; set; } = new();
    public List<PlantViewModel> Plants { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int? TypeId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 6;
    public int TotalPages { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? SearchName { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SeasonId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? RegionId { get; set; }

    public List<Season> Seasons { get; set; } = new();
    public List<SuitableRegion> Regions { get; set; } = new();

    public async Task OnGetAsync()
    {
        PlantTypes = await _context.PlantTypes.ToListAsync();
        Seasons = await _context.Seasons.ToListAsync();
        Regions = await _context.SuitableRegions.ToListAsync();
        var query = _context.Plants
            .Include(p => p.Types)
            .Include(p => p.Plantimages)
            .AsQueryable();

        if (TypeId.HasValue)
        {
            query = query.Where(p => p.Types.Any(t => t.Id == TypeId));
        }
        if (!string.IsNullOrEmpty(SearchName))
        {
            query = query.Where(p => p.PlantName.ToLower().Contains(SearchName.ToLower()));
        }

        if (SeasonId.HasValue)
        {
            query = query.Where(p => p.Seasons.Any(s => s.Id == SeasonId));
        }

        if (RegionId.HasValue)
        {
            query = query.Where(p => p.Regions.Any(r => r.Id == RegionId));
        }


        int totalCount = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        Plants = await query
            .OrderBy(p => p.PlantName)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .Select(p => new PlantViewModel
            {
                Plant = p,
                ImageUrl = _context.Plantimages
                    .Where(img => img.PlantId == p.Id)
                    .Select(img => img.ImageUrl)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }
}