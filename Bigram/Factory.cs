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
using System.IO;

namespace Bigram
{
    public interface IFactory
    {
        IParser CreateParser(BigramFlags flags);
        ICounter CreateCounter(BigramFlags flags);
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
                return new CommandLineTextParser(flags.Args.ToArray(), flags.CrossSentenceBoundaries);
            else if (!string.IsNullOrWhiteSpace(flags.Filepath))
            {
                if (!File.Exists(flags.Filepath))
                    throw new FileNotFoundException("{0) does not exist.  Please correct the file path and try again.", flags.Filepath);

                return new SimpleFileParser(flags.Filepath, flags.CrossSentenceBoundaries);
            }

            throw new System.Exception();
        }


        public ICounter CreateCounter(BigramFlags flags)
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
