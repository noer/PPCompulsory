using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimeTool
{
    public class PrimeTool
    {
        public static bool IsPrime(long candidate)
        {
            // Test whether the parameter is a prime number.
            if ((candidate & 1) == 0)
            {
                if (candidate == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // Note:
            // ... This version was changed to test the square.
            // ... Original version tested against the square root.
            // ... Also we exclude 1 at the end.
            for (int i = 3; (i * i) <= candidate; i += 2)
            {
                if ((candidate % i) == 0)
                {
                    return false;
                }
            }
            return candidate != 1;
        }
        
        public static List<long> GetPrimesSequential(long first, long last)
        {
            var result = new List<long>();

            for (var i = first; i < last; i++)
            {
                if(IsPrime(i))
                    result.Add(i);
            }
            return result;
        }

        public static Task<List<long>> GetPrimesSequentialAsync(long first, long last)
        {
            return Task.Run<List<long>>(() => GetPrimesSequential(first, last));
        }

        public static List<long> GetPrimesParallel(long first, long last)
        {
            var lockObject = new object();
            IEnumerable<long> result = new List<long>();
            Parallel.ForEach(
                Partitioner.Create(first, last),
                () => new List<long>(),
                (range, loopState, partialResult) =>
                {
                    for (long i = range.Item1; i < range.Item2; i++)
                    {
                        if (PrimeTool.IsPrime(i))
                            partialResult.Add(i);
                    }

                    return partialResult;
                },
                (partialResult) =>
                {
                    lock (lockObject)
                    {
                        result = result.Concat(partialResult);
                    }
                }
            );
            return result.OrderBy(s => s).ToList();
        }

        public static Task<List<long>> GetPrimesParallelAsync(long first, long last)
        {
            return Task.Run<List<long>>(() => GetPrimesParallel(first, last));
        }
    }
}