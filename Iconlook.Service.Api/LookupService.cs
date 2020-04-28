using System;
using System.Linq;
using Iconviet;
using Iconviet.Object;
using Iconviet.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class LookupService : ServiceBase
    {
        public object Get(LookupListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                if (request.Filter.HasValue())
                {
                    var filter = request.Filter
                        .Replace("substringof('", string.Empty)
                        .Replace("',tolower(Result))", string.Empty);
                    var preps = redis.As<PRepResponse>().GetAll()
                        .Where(x => x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
                        .Skip(request.Skip).Take(request.Take);
                    return new ListResponse<LookupResponse>(preps.Select(x => new LookupResponse
                    {
                        Result = $"#{x.Ranking}. {x.Name} ({x.Votes:N0} votes)"
                    }))
                    {
                        Skip = request.Skip,
                        Take = request.Take,
                        Count = preps.Count()
                    };
                }
                return new ListResponse<LookupResponse>();
            }
        }
    }
}