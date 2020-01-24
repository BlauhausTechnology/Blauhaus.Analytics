using System;
using System.Threading.Tasks;
using Blauhaus.AppInsights.Test.Tests;

namespace Blauhaus.AppInsights.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new ClientTests().Run();
            
            //await new ServerTests().Run();
        }

    }
}
