using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimTrack.Shared
{
    public record WeightEntryDto(
        int Id,
        DateOnly Date,
        decimal WeightJin,      // 斤
        decimal WeightGongJin,  // 公斤
        decimal? WaistCircumference,  // 腰围(厘米)
        string? Note
    );

    public record UpsertWeightEntryRequest(
        DateOnly Date,
        decimal WeightGongJin,  // 公斤
        decimal? WaistCircumference,  // 腰围(厘米)
        string? Note
    );

    public record BatchImportRequest(
        List<BatchImportItem> Items
    );

    public record BatchImportItem(
        string Date,            // 格式: yyyy.M.d 或 yyyy-MM-dd
        decimal WeightJin,  //
        string? Note
    );
}
