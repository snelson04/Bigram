using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bigram.Core;

namespace Bigram
{
    public interface IFactory
    {
        IParser CreateParser(BigramFlags flags);
        ICounter CreateCounter(BigramFlags flags, IParser parser);
    }

    public class Factory : IFactory
    {
        private static Factory _Singleton { get; set; }

        private Factory()
        {
        }

        public IParser CreateParser(BigramFlags flags)
        {
            if (string.IsNullOrWhiteSpace(flags.Filepath) && flags.Args.Count > 0)
                return new CommandLineArgsParser(flags.Args.ToArray());
            else if (!string.IsNullOrWhiteSpace(flags.Filepath))
            {
                if (!File.Exists(flags.Filepath))
                    throw new FileNotFoundException("{0) does not exist.  Please correct the file path and try again.", flags.Filepath);

                return new SimpleFileParser(flags.Filepath);
            }

            throw new System.Exception();
        }


        public ICounter CreateCounter(BigramFlags flags, IParser parser)
        {
            return new MemoryCounter();
        }

        public static IFactory GetFactory()
        {
            if (Factory._Singleton == null)
                Factory._Singleton = new Factory();

            return Factory._Singleton;
        }
    }
}
