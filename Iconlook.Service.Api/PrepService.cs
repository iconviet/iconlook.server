using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using HtmlAgilityPack;
using Iconlook.Object;
using Serilog;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class PrepService : ServiceBase
    {
        public void Put(PrepListUpdateRequest request)
        {
            Log.Information("Update", request);
        }

        [CacheResponse(Duration = 60, MaxAge = 30)]
        public async Task<object> Get(PrepListRequest request)
        {
            if (Request.GetItem(Keywords.CacheInfo) is CacheInfo cache)
            {
                cache.KeyBase = $"{Request.PathInfo}" +
                                $"&skip={Request.QueryString.Get("skip")}" +
                                $"&take={Request.QueryString.Get("take")}" +
                                $"&edit={Request.QueryString.Get("edit")}" +
                                $"&filter={Request.QueryString.Get("filter")}";
            }
            var response = new ListResponse<PrepResponse>();
            try
            {
                var html = await new HtmlWeb().LoadFromWebAsync("https://icon.community/iconsensus/candidates");
                var query = from t in html.DocumentNode.SelectNodes("//tbody")
                            from r in t.SelectNodes("tr")
                            select r;
                var result = query.Select(x => new PrepResponse
                {
                    Position = new Random().Next(1, 66),
                    Score = new Random().Next(-100, 100),
                    Voters = new Random().Next(100, 1000),
                    Votes = new Random().Next(1000000, 10000000),
                    Direction = new Random().NextDouble() >= 0.5,
                    RejectedBlocks = new Random().Next(100, 1000),
                    LastSeen = $"{new Random().Next(1, 10)} minutes",
                    Created = x.SelectNodes("td")[1].InnerText.Trim(),
                    ProducedBlocks = new Random().Next(100000, 1000000),
                    Name = x.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                    UptimePercentage = new Random().NextDouble() * (0.1 - -0.1) + -0.1,
                    SupplyPercentage = (double) new Random().Next(1000000, 10000000) / 490000000,
                    Id = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3),
                    Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').Last().ToLower().ToTitleCase()
                }).Distinct().OrderBy(x => x.Position).Reverse();
                if (request.Filter.HasValue())
                {
                    var name = request.Filter
                        .Replace("substringof('", string.Empty)
                        .Replace("',tolower(Name))", string.Empty);
                    return new ListResponse<PrepResponse>(result
                        .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                        .Skip(request.Skip).Take(request.Take).ToList());
                }
                if (request.Edit.HasValue() && request.Edit != "all")
                {
                    return new ListResponse<PrepResponse>(result.Where(x => x.Id == request.Edit).ToList())
                    {
                        Skip = 0,
                        Take = 1,
                        Count = 1
                    };
                }
                return new ListResponse<PrepResponse>(result.Skip(request.Skip).Take(request.Take).ToList())
                {
                    Skip = request.Skip,
                    Take = request.Take,
                    Count = query.Count()
                };
            }
            catch (Exception exception)
            {
                Log.Error(exception, exception.Message);
            }
            return response;
        }
    }
}