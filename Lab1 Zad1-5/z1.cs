using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace z1
{
    class Program
    {
		static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 1000 });
            Thread.Sleep(3000);
        }

        static void ThreadProc(Object stateInfo)
        {
            var time = ((object[])stateInfo)[0];
            Thread.Sleep((int)time);
            Console.WriteLine("watek poczekal " + time + " ms");
        }
    }
}
