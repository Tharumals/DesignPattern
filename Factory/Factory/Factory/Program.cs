using System;
using System.Threading.Tasks;

namespace Factory
{
   
public class Foo
    {
        public Foo()
        {
        }
        public  async Task<Foo> InitAsync()
        {
            await Task.Delay(1000);
            return this;
        }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
