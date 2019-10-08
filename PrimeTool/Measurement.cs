using System;
using System.Diagnostics;


namespace PrimeTool
{
    public class Measurement
    {
        public Measurement()
        {
            // Things seem to be faster if the code has been running once already.
            // Provides better consistency in the timings
            WarmUp();

            Console.WriteLine("Check for correctness. Prints numbers from both Sequential and Parallel methods");
            CheckCorrectness();
        
            TestRange(1, 20_000);
            TestRange(1, 1_000_000);
            TestRange(1, 10_000_000);
            TestRange(1_000_000, 2_000_000);
            TestRange(10_000_000, 20_000_000);
        }

        public void TestRange(long first, long last)
        {
            Console.WriteLine("Testing range {0} - {1}", first, last);
            Console.Write("  Sequential:");
            MeasureTime(new Action(() => PrimeTool.GetPrimesSequential(first, last)));
            Console.Write("  Parallel:  ");
            MeasureTime(new Action(() => PrimeTool.GetPrimesParallel(first, last)));
            Console.WriteLine();
        }
        
        public void CheckCorrectness()
        {
            var s = PrimeTool.GetPrimesSequential(1, 40);
            var p = PrimeTool.GetPrimesParallel(1, 40);

            for (int i = 0; i < s.Count; i++)
            {
                Console.WriteLine("{0} == {1}", s[i], p[i]);
            }
            Console.WriteLine();
        }

        public async void WarmUp()
        {
            var s = PrimeTool.GetPrimesSequential(1, 5);
            var p = await PrimeTool.GetPrimesParallelAsync(1, 5);
        }

        private void MeasureTime(Action ac)
        {
            var sw = Stopwatch.StartNew();
            ac.Invoke();
            sw.Stop();
            Console.WriteLine("  Elapsed time: {0:F3}", sw.ElapsedMilliseconds / 1000.0);
        }
    }
}