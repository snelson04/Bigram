using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bigram.Core
{
    public class SimpleFileParser : IParser
    {
        public string Filename { get; private set; }
        public string Seperators { get; private set; }

        /// <summary>
        /// Simple parser that reads a file by line and parses the words using the default or specified seperators
        /// </summary>
        /// <param name="pathname"></param>
        /// <param name="seperators">Defaults to: " ,.;:\t"</param>
        public SimpleFileParser(string pathname, string seperators = "")
        {
            this.Filename = pathname;
            this.Seperators = seperators;

            if (String.IsNullOrEmpty(this.Seperators))
                this.Seperators = " ,.;:\t";

        }

        /// <summary>
        /// Use the passed in counter to track the bigrams found in each line of the file
        /// </summary>
        /// <param name="counter"></param>
        public void Parse(ICounter counter)
        {
            char[] seps = this.Seperators.ToCharArray();
            try
            {
                using (StreamReader input = new StreamReader(Filename))
                {
                    string lastWord = "";
                    while (input.Peek() >= 0)
                    {
                        String line = input.ReadLine();
                        ProcessLine(counter, seps, line, ref lastWord);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Splits the line into words using the passed seps and adds the bigrams to the passed counter
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="seps"></param>
        /// <param name="line"></param>
        /// <param name="lastWord">carry over last word for the next line</param>
        private static void ProcessLine(ICounter counter, char[] seps, string line, ref string lastWord)
        {
            if (!String.IsNullOrWhiteSpace(lastWord))
                line = String.Format("{0} {1}", lastWord, line);

            lastWord = "";

            StringSplitOptions opts = StringSplitOptions.RemoveEmptyEntries;

            string[] words = line.Split(seps, opts);
            for (int idx = 0; idx < words.Length; ++idx)
            {
                if (idx + 1 < words.Length)
                    counter.Add(words[idx], words[idx + 1]);
                else
                    lastWord = words[idx];
            }
        }
    }
}
