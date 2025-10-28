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
        decimal WeightKg,
        string? Note
    );

    public record UpsertWeightEntryRequest(
        DateOnly Date,
        decimal WeightKg,
        string? Note
    );
}
