using System;
using System.Diagnostics;
using System.IO;
using VAE.CLI.Flags;
using Bigram.Core;


namespace Bigram
{
    class Program
    {
        static void Main(string[] args)
        {
            long elapsed = 0;
            try
            {
                /// Replace with DI container (i.e. Ninject/Castle Windsor/Autofac/ etc) when project grows larger
                IFactory factory = Factory.GetFactory();

                BigramFlags flags = new BigramFlags(args);

                if (flags.HasErrors)
                {
                    Usage(flags);
                    return;
                }

                IParser parser = factory.CreateParser(flags);

                ICounter counter = factory.CreateCounter(flags, parser);

                elapsed = PerformParse(parser, counter, flags.Time);

                OutputResults(flags, counter, elapsed);    
            }
            catch(FileNotFoundException ex)
            {

            }
            catch (Exception ex)
            {

            }
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
            throw new NotImplementedException();
        }

        static void Usage(BigramFlags flags)
        {

        }

    }
}
