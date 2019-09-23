using System;
using Agiper.Server;

namespace Iconlook.Service.Job.Prep
{
    public class VoteHistoryPrepJob : JobBase
    {
        public void Run()
        {
            Console.WriteLine("Vote History Prep Job Ran.");
        }
    }
}