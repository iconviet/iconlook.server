using Agiper.Server;
using System.Threading.Tasks;

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