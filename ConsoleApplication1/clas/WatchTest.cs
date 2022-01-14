using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//[assembly: InternalsVisibleTo("ConsoleApplication2")]
namespace ConsoleApplication1
{
    public class WatchTest
    {
        System.Diagnostics.Stopwatch Watch;
        public WatchTest()
        {
            Watch = new System.Diagnostics.Stopwatch();
        }
        public void StartWatch()
        {
            Watch.Reset();
            Watch.Start();
        }

        public double stopWatch()
        {
            Watch.Stop();
            return Watch.Elapsed.TotalMilliseconds;
        }

        public void stopWatchAndShow()
        {
            Watch.Stop();
            Console.WriteLine(Watch.Elapsed.TotalMilliseconds);
            Console.ReadKey();
        }

        /// <summary>
        /// 測試[assembly: InternalsVisibleTo("ConsoleApplication2")]
        /// 在其他方案下 無法取得匿名類的內容 問題 須加上[assembly: InternalsVisibleTo("ConsoleApplication2")]
        /// </summary>
        public dynamic test()
        {
            return new { a = 1, b = 2, c = 3 };
        }

        /// <summary>
        /// 測試ExpandoObject
        /// </summary>
        public dynamic test2()
        {
            dynamic a = new ExpandoObject();
            a.a = 1;
            a.b = 2;
            a.c = 3;
            return a;
        }
    }
}
