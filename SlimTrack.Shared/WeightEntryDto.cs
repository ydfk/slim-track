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
        string? Note
    );

    public record UpsertWeightEntryRequest(
        DateOnly Date,
        decimal WeightGongJin,  // 公斤
        string? Note
    );
}
