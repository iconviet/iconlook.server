using System;
using Agiper.Server;

namespace Iconlook.Service.Web.Jobs
{
    public class BlockProductionStatusJob : JobBase
    {
        public void Run()
        {
            Console.WriteLine("Block Production Status Job Ran.");
        }
    }
}