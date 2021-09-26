using System;
using System.IO;
using System.Threading.Tasks;

namespace PListGen
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "PrimeNumber";
            int digits = 7;

            try
            {
                if (args.Length > 0)
                {
                    digits = Convert.ToInt32(args[0]);
                }
                if (args.Length > 1)
                {
                    path = args[1];
                }
            }
            catch
            {
                Console.WriteLine("usage: plistgen [digits(1-18)] [output_path]");
                return;
            }

            DateTime startTime = DateTime.Now;
            try
            {
                GeneratePrimeNumbers(path, digits);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                DateTime endTime = DateTime.Now;
                Console.WriteLine(endTime - startTime);
            }
        }

        private static void GeneratePrimeNumbers(string path, int digits)
        {
            Directory.CreateDirectory(path);

            long min = 0;
            long max = (long)Math.Pow(10, digits) - 1;

            int slice = 100_000_000;
            if (max + 1 < (long)slice)
            {
                slice = (int)max + 1;
            }

            long maxSlice = (max + 1) / (long)slice;
            long minSlice = min / (long)slice;

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = System.Environment.ProcessorCount;

            object lockObj = new object();
            Parallel.For(minSlice, maxSlice, options, i =>
            {
                long start = i * slice;
                Eratosthenes e = new Eratosthenes(start, slice);
                long count = e.WritePrimes(Path.Combine(path, $"{i * slice}".PadLeft(digits, '0') + ".txt"), lockObj, digits);
                Console.WriteLine($"{start}-{start - 1 + (long)slice}: {count}");
            });
        }
    }
}
