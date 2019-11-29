using Agiper.Server;
using System.Threading.Tasks;

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