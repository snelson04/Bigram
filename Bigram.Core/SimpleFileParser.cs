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

using System;
using System.IO;

namespace Bigram.Core
{
    public class SimpleFileParser : BaseParser, IParser
    {
        public string Filename { get; private set; }
        public string Seperators { get; private set; }

        /// <summary>
        /// Simple parser that reads a file by line and parses the words using the default or specified seperators
        /// </summary>
        /// <param name="pathname"></param>
        /// <param name="seperators">Defaults to: " ,.;:\t"</param>
        public SimpleFileParser(string pathname, bool crossSentenceBoundaries = false, string seperators = "") : base(crossSentenceBoundaries)
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
                        this.ProcessLine(counter, seps, line, ref lastWord);
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
        private void ProcessLine(ICounter counter, char[] seps, string line, ref string lastWord)
        {
            if (!String.IsNullOrWhiteSpace(lastWord))
                line = String.Format("{0} {1}", lastWord, line);

            lastWord = "";


            StringSplitOptions opts = this._crossSentenceBoundaries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;

            string[] words = line.Split(seps, opts);
            for (int idx = 0; idx < words.Length; ++idx)
            {
                if (!this._crossSentenceBoundaries && this.IsEndOfSentence(words[idx]))
                    continue;

                if (idx + 1 < words.Length)
                {
                    string word1 = words[idx];
                    string word2 = words[idx + 1];

                    if(!string.IsNullOrWhiteSpace(word1)
                        && !string.IsNullOrWhiteSpace(word2))
                                  counter.Add(words[idx], words[idx + 1]);
                }
                else
                    lastWord = words[idx].Trim();
            }
        }
    }
}
