using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeTool
{
    public class Measurement
    {
        public Measurement()
        {
            //TestCorrectness();
            
            WarmUp();
            Console.WriteLine("Prime numbers Sequential");
            MeasureTime(new Action(() => PrimeTool.GetPrimesSequential(1_000_000, 1_002_000)));

            Console.WriteLine("Prime numbers Parallel");
            MeasureTime(new Action(() => PrimeTool.GetPrimesParallel(1_000_000, 1_002_000)));
        }

        public void TestCorrectness()
        {
            var s = PrimeTool.GetPrimesSequential(1, 1_000);
            var p = PrimeTool.GetPrimesParallel(1, 1_000);
            
            for (int i = 0; i < s.Count; i++)
            {
                Console.WriteLine("{0:F0}, {1:F0}", s[i], p[i]);
            }
        }

        public void WarmUp()
        {
            var s = PrimeTool.GetPrimesSequential(1, 5);
            var p = PrimeTool.GetPrimesParallel(1, 5);
        }

        private void MeasureTime(Action ac)
        {
            var sw = Stopwatch.StartNew();
            ac.Invoke();
            sw.Stop();
            Console.WriteLine("  Elapsed time: {0:F5}", sw.ElapsedMilliseconds / 1000d);
        }
    }
}