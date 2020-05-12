using System.Linq;
using Iconlook.Object;
using Iconviet;
using Iconviet.Object;
using Iconviet.Server;
using Serilog;

namespace Iconlook.Service.Api
{
    public class PRepService : ServiceBase
    {
        public void Put(PRepListUpdateRequest request)
        {
            Log.Information("Update", request);
        }

        public object Get(PRepListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var items = redis.As<PRepResponse>().GetAll();
                if (request.Edit.HasValue() && request.Edit != "all")
                {
                    return new ListResponse<PRepResponse>(items.Where(x => x.Id == request.Edit))
                    {
                        Skip = 0,
                        Take = 1,
                        Count = 1
                    };
                }
                if (request.Filter.HasValue())
                {
                    switch (request.Filter)
                    {
                        case "main_prep":
                            items = items.Where(x => x.Ranking <= 22).ToList();
                            break;
                        case "candidate":
                            items = items.Where(x => x.Ranking > 100).ToList();
                            break;
                        case "controversal":
                            items = items.Where(x => x.Name == "controversal").ToList();
                            break;
                        case "disqualified":
                            items = items.Where(x => x.Name == "disqualified").ToList();
                            break;
                        case "sub_prep":
                            items = items.Where(x => x.Ranking > 22 && x.Ranking <= 100).ToList();
                            break;
                    }
                }
                return new ListResponse<PRepResponse>(items
                    .OrderBy(x => x.Ranking).Skip(items.Count > request.Take ? request.Skip : 0).Take(request.Take))
                {
                    Count = items.Count,
                    Take = request.Take,
                    Skip = items.Count > request.Take ? request.Skip : 0
                };
            }
        }
    }
}