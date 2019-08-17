using System.Threading.Tasks;
using Agiper.Server;

namespace Iconlook.Service.Api
{
    public class Program : ProgramBase
    {
        public static async Task Main()
        {
            await StartAsync(new ApiHost(), 81);
        }
    }
}