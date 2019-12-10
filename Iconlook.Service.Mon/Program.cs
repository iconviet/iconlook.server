using System.Threading.Tasks;
using Agiper.Server;

namespace Iconlook.Service.Mon
{
    public class Program : ProgramBase
    {
        public static Task Main()
        {
            return StartAsync(new MonHost(), 83);
        }
    }
}