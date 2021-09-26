using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace PListGen
{
    public class Eratosthenes
    {
        public long Start { get; private set; }
        public int Length { get; private set; }

        private int[] buffer = null;
        private long[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31 };
        public Eratosthenes(long start, int length)
        {
            Start = start;
            Length = length;
        }
        /// <summary>
        /// Output prime numbers to a file. To ensure continuity when writing
        /// to the HDD, file writing is done in series.
        /// </summary>
        /// <param name="filename">File name</param>
        /// <param name="lockObj">Lock object to write file</param>
        /// <param name="digits">Number of digits</param>
        /// <returns>The number of prime numbers that were in this range</returns>
        public long WritePrimes(string filename, object lockObj, int digits)
        {
            // initialize buffer
            buffer = ArrayPool<int>.Shared.Rent(Length);
            for (int i = 0; i < Length; i++)
            {
                buffer[i] = 0;
            }

            long max = Start + Length;
            long sqrtMax = (long)Math.Sqrt(max);
            if (Start < 2)
            {
                for (int i = 0; i < 2 - Start; i++)
                {
                    buffer[i] = 1; // 1: not prime number
                }
            }

            for (long i = 2; i <= sqrtMax; i++)
            {
                if (IsCompositeNumber(i))
                {
                    continue;
                }
                long start = i * (long)Math.Ceiling(1.0 * Start / i);
                for (long j = start; j < max; j += i)
                {
                    if (j != i && j > Start)
                    {
                        int index = (int)(j - Start);
                        buffer[index] = 1;
                    }
                }
            }

            // To ensure continuity when writing to the HDD, file writing is done in series.
            lock (lockObj)
            {
                int count = 0;
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    for (int i = 0; i < Length; i++)
                    {
                        if (buffer[i] != 1)
                        {
                            sw.WriteLine($"{i + Start}".PadLeft(digits, '0'));
                            count++;
                        }
                    }
                }
                ArrayPool<int>.Shared.Return(buffer);
                return count;
            }
        }

        /// <summary>
        ///  simple composite number check to speed up the calculation
        /// </summary>
        private bool IsCompositeNumber(long num)
        {
            for (int i = 0; i < primes.Length; i++)
            {
                if (primes[i] < num && (num % primes[i]) == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
