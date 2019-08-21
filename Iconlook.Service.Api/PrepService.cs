using System;
using System.Threading.Tasks;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Entity;
using Iconlook.Object;
using Serilog;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Iconlook.Service.Api
{
    public class PrepService : ServiceBase
    {
        public void Put(PrepListUpdateRequest request)
        {
            Log.Information("Update", request);
        }

        public async Task<object> Get(PrepListRequest request)
        {
            if (request.Filter.HasValue())
            {
                var name = request.Filter
                    .Replace("substringof('", string.Empty)
                    .Replace("',tolower(Name))", string.Empty);
                var preps = await Db.SelectAsync(Db.From<Prep>().Where(x =>
                    x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));
                return new ListResponse<PrepResponse>(preps.ConvertAll(x => x.ConvertTo<PrepResponse>()))
                {
                    Skip = request.Skip, Take = request.Take, Count = preps.Count
                };
            }
            if (request.Edit.HasValue() && request.Edit != "all")
            {
                var preps = await Db.SelectAsync(
                    Db.From<Prep>().Where(x => x.IdExternal == request.Edit));
                return new ListResponse<PrepResponse>(preps.ConvertAll(x => x.ConvertTo<PrepResponse>()))
                {
                    Skip = 0, Take = 1, Count = 1
                };
            }
            {
                var query = Db.From<Prep>();
                if (request.Take > 0) query.Take(request.Take);
                var preps = await Db.SelectAsync(query.Skip(0).OrderBy(x => x.Position));
                return new ListResponse<PrepResponse>(preps.ConvertAll(x => x.ConvertTo<PrepResponse>()))
                {
                    Skip = request.Skip, Take = request.Take, Count = preps.Count
                };
            }
        }
    }
}