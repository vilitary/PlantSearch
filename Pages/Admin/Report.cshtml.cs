using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using PlantSearch.Models;
using System.IO;

public class ReportModel : PageModel
{
    private readonly Tree3Context _context;
    public ReportModel(Tree3Context context) => _context = context;

    public List<string> MonthLabels { get; set; } = new();
    public List<int> MonthlyPlantCounts { get; set; } = new();
    public List<string> TypeLabels { get; set; } = new();
    public List<int> TypeCounts { get; set; } = new();

    [BindProperty] public string? ChartMonthImage { get; set; }
    [BindProperty] public string? ChartTypeImage { get; set; }

    public async Task OnGetAsync()
    {
        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;

        for (int month = 1; month <= currentMonth; month++)
        {
            MonthLabels.Add($"Tháng {month}");
            int count = await _context.Plants
                .Where(p => p.CreatedAt.HasValue &&
                            p.CreatedAt.Value.Month == month &&
                            p.CreatedAt.Value.Year == currentYear)
                .CountAsync();
            MonthlyPlantCounts.Add(count);
        }

        var typeStats = await _context.PlantTypes
            .Select(t => new { t.TypeName, Count = t.Plants.Count() })
            .ToListAsync();

        foreach (var item in typeStats)
        {
            TypeLabels.Add(item.TypeName);
            TypeCounts.Add(item.Count);
        }


    }
    public IActionResult OnPost([FromForm] string ChartMonthImage, [FromForm] string ChartTypeImage)
    {
        if (ChartMonthImage?.StartsWith("data:image/png;base64,") == true)
            ChartMonthImage = ChartMonthImage.Replace("data:image/png;base64,", "");

        if (ChartTypeImage?.StartsWith("data:image/png;base64,") == true)
            ChartTypeImage = ChartTypeImage.Replace("data:image/png;base64,", "");

        var stream = new MemoryStream();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Column(col =>
                {
                    col.Item().Text("📊 Thống kê cây trồng theo tháng").FontSize(18).Bold();

                    if (!string.IsNullOrEmpty(ChartMonthImage))
                    {
                        try
                        {
                            var bytes = Convert.FromBase64String(ChartMonthImage);
                            col.Item().Image(bytes);
                        }
                        catch
                        {
                            col.Item().Text("⚠ Không thể hiển thị biểu đồ tháng.");
                        }
                    }

                    col.Item().PaddingTop(20);
                    col.Item().Text("🌿 Thống kê cây theo loại").FontSize(18).Bold();

                    if (!string.IsNullOrEmpty(ChartTypeImage))
                    {
                        try
                        {
                            var bytes = Convert.FromBase64String(ChartTypeImage);
                            col.Item().Image(bytes);
                        }
                        catch
                        {
                            col.Item().Text("⚠ Không thể hiển thị biểu đồ loại.");
                        }
                    }
                });
            });
        });

        document.GeneratePdf(stream);
        stream.Position = 0;
        return File(stream, "application/pdf", "ThongKeCayTrong.pdf");
    }


}
