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
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapControllers();

// 列表,支持日期范围过滤
app.MapGet("/api/weights", async (AppDbContext db, DateOnly? start, DateOnly? end) =>
{
    var q = db.WeightEntries.AsQueryable();
    if (start.HasValue) q = q.Where(x => x.Date >= start);
    if (end.HasValue) q = q.Where(x => x.Date <= end);

    var list = await q
        .OrderBy(x => x.Date)
        .Select(x => new WeightEntryDto(x.Id, x.Date, x.WeightJin, x.WeightGongJin, x.WaistCircumference, x.Note))
        .ToListAsync();
    return Results.Ok(list);
});

// 统计(最近 N 天)
app.MapGet("/api/weights/stats", async (AppDbContext db, int days) =>
{
    var since = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-days));
    var data = await db.WeightEntries
        .Where(x => x.Date >= since)
        .OrderBy(x => x.Date)
        .Select(x => new { x.Date, x.WeightGongJin, x.WeightJin, x.WaistCircumference })
        .ToListAsync();

    var min = data.Any() ? data.Min(x => x.WeightGongJin) : 0;
    var max = data.Any() ? data.Max(x => x.WeightGongJin) : 0;
    var avg = data.Any() ? Math.Round(data.Average(x => (double)x.WeightGongJin), 2) : 0;

    return Results.Ok(new { min, max, avg, points = data });
});

// 新增/更新(Upsert,按 Date 唯一键)
app.MapPost("/api/weights", async (AppDbContext db, UpsertWeightEntryRequest req) =>
{
    var entity = await db.WeightEntries.FirstOrDefaultAsync(x => x.Date == req.Date);
    if (entity is null)
    {
        entity = new WeightEntry
        {
            Date = req.Date,
            WeightGongJin = req.WeightGongJin,
            WeightJin = Math.Round(req.WeightGongJin * 2, 2), // 1公斤=2斤
            WaistCircumference = req.WaistCircumference,
            Note = req.Note
        };
        db.WeightEntries.Add(entity);
    }
    else
    {
        entity.WeightGongJin = req.WeightGongJin;
        entity.WeightJin = Math.Round(req.WeightGongJin * 2, 2); // 1公斤=2斤
        entity.WaistCircumference = req.WaistCircumference;
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

// 批量导入
app.MapPost("/api/weights/batch-import", async (AppDbContext db, BatchImportRequest req) =>
{
    var results = new List<object>();
    var errors = new List<string>();

    foreach (var item in req.Items)
    {
        try
        {
            // 解析日期,支持 yyyy.M.d 或 yyyy-MM-dd 格式
            DateOnly date;
            if (item.Date.Contains('.'))
            {
                var parts = item.Date.Split('.');
                if (parts.Length != 3 || !int.TryParse(parts[0], out var year) ||
                    !int.TryParse(parts[1], out var month) || !int.TryParse(parts[2], out var day))
                {
                    errors.Add($"日期格式错误: {item.Date}");
                    continue;
                }
                date = new DateOnly(year, month, day);
            }
            else
            {
                if (!DateOnly.TryParse(item.Date, out date))
                {
                    errors.Add($"日期格式错误: {item.Date}");
                    continue;
                }
            }

            // 查找或创建
            var entity = await db.WeightEntries.FirstOrDefaultAsync(x => x.Date == date);
            if (entity is null)
            {
                entity = new WeightEntry
                {
                    Date = date,
                    WeightJin = item.WeightJin,
                    WeightGongJin = Math.Round(item.WeightJin / 2, 2),
                    Note = item.Note
                };
                db.WeightEntries.Add(entity);
                results.Add(new { Date = date.ToString("yyyy-MM-dd"), Status = "Created", WeightGongJin = item.WeightJin });
            }
            else
            {
                entity.WeightJin = item.WeightJin;
                entity.WeightGongJin = Math.Round(item.WeightJin / 2, 2);
                entity.Note = item.Note;
                entity.UpdatedAt = DateTime.UtcNow;
                results.Add(new { Date = date.ToString("yyyy-MM-dd"), Status = "Updated", WeightGongJin = item.WeightJin });
            }
        }
        catch (Exception ex)
        {
            errors.Add($"处理失败 {item.Date}: {ex.Message}");
        }
    }

    await db.SaveChangesAsync();
    return Results.Ok(new
    {
        TotalProcessed = results.Count,
        TotalErrors = errors.Count,
        Results = results,
        Errors = errors
    });
});


// 回退到 SPA 的前端路由
app.MapFallbackToFile("index.html");

app.Run();




