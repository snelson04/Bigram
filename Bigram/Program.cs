/*
Copyright 2018 Shawn Nelson

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/


using Bigram.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace Bigram
{
    class Program
    {
        static void Main(string[] args)
        {
            long elapsed = 0;
            BigramFlags flags = new BigramFlags(args);

            try
            {
                /// Replace with DI container (i.e. Ninject/Castle Windsor/Autofac/ etc) when project grows larger
                IFactory factory = Factory.GetFactory();

                if (flags.HasErrors)
                {
                    Console.WriteLine("Error in command line arguments: {0}\r\n", flags.ErrorText);
                    DisplayUsage(flags);
                    return;
                }

                IParser parser = factory.CreateParser(flags);

                ICounter counter = factory.CreateCounter(flags, parser);

                elapsed = PerformParse(parser, counter, flags.Time);

                OutputResults(flags, counter, elapsed);    
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine(string.Format("Unable to find the file specified: {0}", flags.Filepath));
                DisplayUsage(flags);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Unexpected error encountered during execution: {0}", ex.Message));
                DisplayUsage(flags);
            }

            return;
        }

        private static long PerformParse(IParser parser, ICounter counter, bool countElapsedTime)
        {
            long startTS = 0;
            long elapsed = 0;

            if(countElapsedTime)
            startTS = Stopwatch.GetTimestamp();

            parser.Parse(counter);

            if (countElapsedTime)
                elapsed = Stopwatch.GetTimestamp() - startTS;

            return elapsed;
        }

        private static void OutputResults(BigramFlags flags, ICounter counter, long elapsed)
        {
            List<BigramCountValue> counts = counter.BigramCountList();
            
            switch(flags.Order)
            {
                case "alpha":
                    counts.Sort((x, y) => x.Item1.CompareTo(y.Item1));
                    break;
                case "freq":
                    counts.Sort((x, y) => x.Item2.CompareTo(y.Item2));
                    break;
                case "freq_d":
                    counts.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                    break;
                default:
                    break;
            }

            foreach(BigramCountValue val in counts)
            {
                Console.WriteLine(string.Format("{0, -20}\t{1, 10}", val.Item1, val.Item2));
            }

            if (elapsed > 0)
            {
                Console.WriteLine();
                Console.WriteLine(string.Format("Elapsed time: {0} / {1} s", elapsed, ((double)elapsed / Stopwatch.Frequency)));
            }
        }

        static void DisplayUsage(BigramFlags flags)
        {
            Process myProcess = System.Diagnostics.Process.GetCurrentProcess();
            string displayName = Path.GetFileNameWithoutExtension(myProcess.MainModule.FileName);
            Console.WriteLine(string.Format("USAGE: {0} [options]  | Use -filepath option to point to a valid file to process", displayName));
            Console.WriteLine(string.Format("   OR: {0} [options] text to process... | Do NOT include the -filepath option", displayName));
            Console.WriteLine("===============================================================================================");
            Console.WriteLine("OPTIONS:");
            foreach (string line in flags.HelpText)
                Console.WriteLine(line);


            myProcess.Dispose();        }

    }
}
