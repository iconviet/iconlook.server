using System.Threading.Tasks;
using Agiper.Server;

namespace Iconlook.Service.Job
{
    public class Program : ProgramBase
    {
        public static Task Main()
        {
            return StartAsync(new JobHost(), 82);
        }
    }
}