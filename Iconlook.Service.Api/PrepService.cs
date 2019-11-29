using System;
using System.Threading.Tasks;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using Serilog;
using ServiceStack.OrmLite;

namespace Iconlook.Service.Api
{
    public class PRepService : ServiceBase
    {
        public void Put(PRepListUpdateRequest request)
        {
            Log.Information("Update", request);
        }

        public async Task<object> Get(PRepListRequest request)
        {
            if (request.Filter.HasValue())
            {
                var name = request.Filter
                    .Replace("substringof('", string.Empty)
                    .Replace("',tolower(Name))", string.Empty);
                var preps = await Db.SelectAsync(Db.From<PRep>().Where(x =>
                    x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));
                return new ListResponse<PRepResponse>(preps.ConvertAll(x => x.ToResponse()))
                {
                    Skip = request.Skip,
                    Take = request.Take,
                    Count = preps.Count
                };
            }
            if (request.Edit.HasValue() && request.Edit != "all")
            {
                var preps = await Db.SelectAsync(
                    Db.From<PRep>().Where(x => x.IdExternal == request.Edit));
                return new ListResponse<PRepResponse>(preps.ConvertAll(x => x.ToResponse()))
                {
                    Skip = 0,
                    Take = 1,
                    Count = 1
                };
            }
            {
                var query = Db.From<PRep>();
                if (request.Skip > 0) query.Skip(request.Skip);
                if (request.Take > 0) query.Take(request.Take);
                var preps = await Db.SelectAsync(query.OrderBy(x => x.Position));
                var response = new ListResponse<PRepResponse>(preps.ConvertAll(x => x.ToResponse()))
                {
                    Skip = request.Skip,
                    Take = request.Take,
                    Count = await Db.CountAsync<PRep>()
                };
                return response;
            }
        }
    }
}