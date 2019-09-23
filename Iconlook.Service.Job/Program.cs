using System.Threading.Tasks;
using Agiper.Server;

namespace Iconlook.Service.Job
{
    public class Program : ProgramBase
    {
        public static async Task Main()
        {
            await StartAsync(new JobHost(), 82);
        }
    }
}