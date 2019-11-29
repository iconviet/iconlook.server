using System.Threading.Tasks;
using Agiper.Server;

namespace Iconlook.Service.Api
{
    public class Program : ProgramBase
    {
        public static Task Main()
        {
            return StartAsync(new ApiHost(), 81);
        }
    }
}