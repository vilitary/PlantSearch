using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlantSearch.Models;

public class AddPlantModel : PageModel
{
    private readonly Tree3Context _context;

    public AddPlantModel(Tree3Context context)
    {
        _context = context;
    }

    [BindProperty] public Plant Plant { get; set; } = new();
    [BindProperty] public string? NewImageUrl { get; set; }

    [BindProperty] public List<int> SelectedSoilIds { get; set; } = new();
    [BindProperty] public List<int> SelectedSeasonIds { get; set; } = new();
    [BindProperty] public List<int> SelectedFertilizerIds { get; set; } = new();
    [BindProperty] public List<int> SelectedDiseaseIds { get; set; } = new();
    [BindProperty] public List<int> SelectedRegionIds { get; set; } = new();
    [BindProperty] public List<int> SelectedMethodIds { get; set; } = new();
    [BindProperty] public List<int> SelectedTypeIds { get; set; } = new();

    public List<SoilType> SoilTypes { get; set; } = new();
    public List<Season> Seasons { get; set; } = new();
    public List<FertilizerType> AllFertilizers { get; set; } = new();
    public List<DiseaseType> AllDiseases { get; set; } = new();
    public List<SuitableRegion> Regions { get; set; } = new();
    public List<CultivationMethod> AllMethods { get; set; } = new();
    public List<PlantType> Categories { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        SoilTypes = await _context.SoilTypes.ToListAsync();
        Seasons = await _context.Seasons.ToListAsync();
        AllFertilizers = await _context.FertilizerTypes.ToListAsync();
        AllDiseases = await _context.DiseaseTypes.ToListAsync();
        Regions = await _context.SuitableRegions.ToListAsync();
        AllMethods = await _context.CultivationMethods.ToListAsync();
        Categories = await _context.PlantTypes.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Plant.CreatedAt = DateTime.Now;
        Plant.UpdatedAt = DateTime.Now;
        _context.Plants.Add(Plant);
        await _context.SaveChangesAsync();

        if (!string.IsNullOrEmpty(NewImageUrl))
        {
            _context.Plantimages.Add(new Plantimage
            {
                PlantId = Plant.Id,
                ImageUrl = NewImageUrl
            });
        }

        foreach (var id in SelectedSoilIds)
            _context.PlantSoils.Add(new PlantSoil { PlantId = Plant.Id, SoilId = id });

        foreach (var id in SelectedSeasonIds)
            _context.PlantSeasons.Add(new PlantSeason { PlantId = Plant.Id, SeasonId = id });

        foreach (var id in SelectedFertilizerIds)
            _context.PlantFertilizers.Add(new PlantFertilizer { PlantId = Plant.Id, FertilizerId = id });

        foreach (var id in SelectedDiseaseIds)
            _context.PlantDiseases.Add(new PlantDisease { PlantId = Plant.Id, DiseaseId = id });

        foreach (var id in SelectedRegionIds)
            _context.PlantRegions.Add(new PlantRegion { PlantId = Plant.Id, RegionId = id });

        foreach (var id in SelectedMethodIds)
            _context.PlantMethods.Add(new PlantMethod { PlantId = Plant.Id, MethodId = id });

        Plant.Types = await _context.PlantTypes
     .Where(t => SelectedTypeIds.Contains(t.Id))
     .ToListAsync();


        await _context.SaveChangesAsync();
        return RedirectToPage("/Admin/Plants");
    }
}
