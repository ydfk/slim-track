using SlimTrack.Shared;
using System.Net.Http.Json;

namespace SlimTrack.Client.Services
{
    public class WeightService(HttpClient http)
    {
        public async Task<List<WeightEntryDto>> List(DateOnly? start = null, DateOnly? end = null)
        {
            var url = "/api/weights";
            var qs = new List<string>();
            if (start.HasValue) qs.Add($"start={start}");
            if (end.HasValue) qs.Add($"end={end}");
            if (qs.Count > 0) url += "?" + string.Join("&", qs);
            return await http.GetFromJsonAsync<List<WeightEntryDto>>(url) ?? [];
        }

        public Task<HttpResponseMessage> Upsert(UpsertWeightEntryRequest req)
            => http.PostAsJsonAsync("/api/weights", req);

        public Task<HttpResponseMessage> Delete(int id)
            => http.DeleteAsync($"/api/weights/{id}");

        public Task<StatsResponse?> Stats(int days = 30)
            => http.GetFromJsonAsync<StatsResponse>($"/api/weights/stats?days={days}");
    }

    public record StatsResponse(decimal min, decimal max, double avg, List<Point> points);

    public record Point(DateOnly date, decimal weightGongJin, decimal weightJin);
}
