using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using HtmlAgilityPack;
using Iconlook.Message;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class PrepService : ServiceBase
    {
        public void Put(PrepUpdateRequest request)
        {
        }

        [CacheResponse(Duration = 60, MaxAge = 30)]
        public async Task<object> Get(PrepListRequest request)
        {
            if (Request.GetItem(Keywords.CacheInfo) is CacheInfo cache)
            {
                cache.KeyBase = $"{Request.PathInfo}" +
                                $"?take={Request.QueryString.Get("take")}" +
                                $"&edit={Request.QueryString.Get("edit")}";
                if (await Request.HandleValidCache(cache))
                {
                    return null;
                }
            }
            var response = new ListResponse<PrepResponse>();
            try
            {
                var html = await new HtmlWeb().LoadFromWebAsync("https://icon.community/iconsensus/candidates");
                var query = from t in html.DocumentNode.SelectNodes("//tbody")
                            from r in t.SelectNodes("tr")
                            select r;
                var position = new Stack<int>(Enumerable.Range(1, query.Count()));
                var result = query.Select(x => new PrepResponse
                {
                    Position = position.Pop(),
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
                    Id = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3),
                    Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').LastOrDefault().ToLower().ToTitleCase()
                }.ThenDo(o => o.SupplyPercentage = (double) o.Votes / 490000000)).Distinct().OrderBy(x => x.Position).Take(request.Take).Reverse();
                var id = Request.QueryString.Get("edit");
                if (id.HasValue() && id != "all")
                {
                    result = result.Where(x => x.Id == id);
                }
                response = new ListResponse<PrepResponse>(result.ToList());
            }
            catch (Exception)
            {
            }
            return response;
        }
    }
}