using System;

namespace CsharpSandbox.ObjectPooling
{
    internal class RandomGenerator : IRandomDependency
    {
        private readonly Random _random = new Random();
        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
