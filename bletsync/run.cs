using bletsync.src;
using System.Text;

namespace bletsync;
class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Watcher watcher = new(@"");
        watcher.Start();
    }
}