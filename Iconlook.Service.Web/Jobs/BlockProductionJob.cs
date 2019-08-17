using System;
using Agiper.Server;

namespace Iconlook.Service.Web.Jobs
{
    public class BlockProductionJob : JobBase
    {
        public void Run()
        {
            Console.WriteLine("Block Production Monitor Job Ran.");
        }
    }
}