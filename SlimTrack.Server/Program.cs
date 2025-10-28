using Microsoft.AspNetCore.Components.WebAssembly.Server; // ���Ӵ� using ָ��
using Microsoft.EntityFrameworkCore;
using SlimTrack.Server;
using SlimTrack.Shared;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// DbContext
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "appdata", "slimtrack.db");
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

// Ensure database schema is up to date before handling requests
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

// �� �й� Blazor WASM ��̬��Դ������ Client ��Ŀ��
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

// �� API ·�ɣ�������� Minimal API/Controller��
app.MapControllers();
// ���ﲻҪ�� MapGet("/")���ᵲסǰ�����


// �б���֧�����ڷ�Χ����
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

// ͳ�ƣ���� N �죩��
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

// ����/���£�Upsert���� Date Ψһ����
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

// ɾ��
app.MapDelete("/api/weights/{id:int}", async (AppDbContext db, int id) =>
{
    var e = await db.WeightEntries.FindAsync(id);
    if (e is null) return Results.NotFound();
    db.Remove(e);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


// �� SPA ���˵�ǰ�����
app.MapFallbackToFile("index.html");

app.Run();




