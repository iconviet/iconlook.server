using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using HtmlAgilityPack;
using Iconlook.Entity;
using Iconlook.Message;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class PRepService : ServiceBase
    {
        public async Task<List<PRepResponse>> Any(PRepListRequest request)
        {
            var data = new List<PRepResponse>();
            try
            {
                var html = await new HtmlWeb().LoadFromWebAsync("https://icon.community/iconsensus/candidates");
                var query = from t in html.DocumentNode.SelectNodes("//tbody")
                            from r in t.SelectNodes("tr")
                            select r;
                var rank = new Stack<int>(Enumerable.Range(1, query.Count()));
                data = query.Select(x => new PRepResponse
                {
                    Rank = rank.Pop(),
                    Created = x.SelectNodes("td")[1].InnerText.Trim(),
                    Name = x.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                    Id = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3),
                    Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').LastOrDefault().ToLower().ToTitleCase(),
                    LogoUrl = $"images/preps/{x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3)}.png"
                }).OrderBy(x => x.Rank).Reverse().Take(request.Take == 0 ? 22 : 0).ToList();
            }
            catch (Exception)
            {
            }
            return data;
        }

        public async Task<List<PRep>> GetLatestPRepsAsync(int take = 22)
        {
            var data = new List<PRep>();
            try
            {
                var html = await new HtmlWeb().LoadFromWebAsync("https://icon.community/iconsensus/candidates");
                var query = from t in html.DocumentNode.SelectNodes("//tbody")
                            from r in t.SelectNodes("tr")
                            select r;
                var rank = new Stack<int>(Enumerable.Range(1, query.Count()));
                data = query.Select(x => new PRep
                {
                    Rank = rank.Pop(),
                    Created = x.SelectNodes("td")[1].InnerText.Trim(),
                    Name = x.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                    Id = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3),
                    Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').LastOrDefault().ToLower().ToTitleCase(),
                    LogoUrl = $"images/preps/{x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3)}.png"
                }).OrderBy(x => x.Rank).Reverse().Take(take).ToList();
            }
            catch (Exception)
            {
            }
            return data;
        }
    }
}