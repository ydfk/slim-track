using Microsoft.AspNetCore.Components.WebAssembly.Server; // 添加此 using 指令
using Microsoft.EntityFrameworkCore;
using SlimTrack.Server;
using SlimTrack.Shared;

var builder = WebApplication.CreateBuilder(args);

// DbContext
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "appdata", "slimtrack.db");
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

// ★ 托管 Blazor WASM 静态资源（来自 Client 项目）
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// ★ API 路由（如果你有 Minimal API/Controller）
app.MapControllers();
// 这里不要再 MapGet("/")！会挡住前端入口


// 列表（支持日期范围）：
app.MapGet("/api/weights", async (AppDbContext db, DateOnly? start, DateOnly? end) =>
{
    var q = db.WeightEntries.AsQueryable();
    if (start.HasValue) q = q.Where(x => x.Date >= start);
    if (end.HasValue) q = q.Where(x => x.Date <= end);

    var list = await q
        .OrderBy(x => x.Date)
        .Select(x => new WeightEntryDto(x.Id, x.Date, x.WeightKg, x.Note))
        .ToListAsync();
    return Results.Ok(list);
});

// 统计（最近 N 天）：
app.MapGet("/api/weights/stats", async (AppDbContext db, int days) =>
{
    var since = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-days));
    var data = await db.WeightEntries
        .Where(x => x.Date >= since)
        .OrderBy(x => x.Date)
        .Select(x => new { x.Date, x.WeightKg })
        .ToListAsync();

    var min = data.Any() ? data.Min(x => x.WeightKg) : 0;
    var max = data.Any() ? data.Max(x => x.WeightKg) : 0;
    var avg = data.Any() ? Math.Round(data.Average(x => (double)x.WeightKg), 2) : 0;

    return Results.Ok(new { min, max, avg, points = data });
});

// 新增/更新（Upsert：按 Date 唯一）：
app.MapPost("/api/weights", async (AppDbContext db, UpsertWeightEntryRequest req) =>
{
    var entity = await db.WeightEntries.FirstOrDefaultAsync(x => x.Date == req.Date);
    if (entity is null)
    {
        entity = new WeightEntry { Date = req.Date, WeightKg = req.WeightKg, Note = req.Note };
        db.WeightEntries.Add(entity);
    }
    else
    {
        entity.WeightKg = req.WeightKg;
        entity.Note = req.Note;
        entity.UpdatedAt = DateTime.UtcNow;
    }
    await db.SaveChangesAsync();
    return Results.Ok(new { entity.Id });
});

// 删除
app.MapDelete("/api/weights/{id:int}", async (AppDbContext db, int id) =>
{
    var e = await db.WeightEntries.FindAsync(id);
    if (e is null) return Results.NotFound();
    db.Remove(e);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


// ★ SPA 回退到前端入口
app.MapFallbackToFile("index.html");

app.Run();

