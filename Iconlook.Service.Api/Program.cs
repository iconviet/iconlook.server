using System.Threading.Tasks;

namespace Iconlook.Service.Api
{
    public class Program : Agiper.Server.Program
    {
        public static async Task Main()
        {
            await StartAsync<ApiHost>("http://*:81");
        }
    }
}