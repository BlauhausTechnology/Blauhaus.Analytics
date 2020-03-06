using System;
using System.Threading.Tasks;

namespace Samples.Xamarin
{
    public class NumberGenerator
    {
        public async Task<int> GenerateAsync()
        {
            var random = new Random().Next();

            await Task.Delay(2000);

            return random;
        }
    }
}