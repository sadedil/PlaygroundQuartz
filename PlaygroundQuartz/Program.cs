using System;
using System.Threading.Tasks;

namespace PlaygroundQuartz
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ex = new SimpleExample();
            Task.WaitAll(ex.Run());
        }
    }
}