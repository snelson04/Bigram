using System;
using System.Collections.Generic;
using System.Text;

namespace Bigram.Core
{
    public class CommandLineArgsParser : IParser
    {
        private string[] _args;
        private string _sentenceBoundaries = ".!?";
        private bool _crossSentenceBoundaries;

        public CommandLineArgsParser(string[] args, bool crossSentenceBoundaries = false)
        {
            this._args = args;
            this._crossSentenceBoundaries = crossSentenceBoundaries;
        }

        public void Parse(ICounter counter)
        {
            for (int idx = 0; idx < this._args.Length; ++idx)
            {
                bool isSentenceEnd = this.IsEndOfSentence(this._args[idx]);
                if (idx + 1 < this._args.Length)
                {
                    if (!this._crossSentenceBoundaries && isSentenceEnd)
                        continue;

                    string word1 = this._args[idx];
                    string word2 = this._args[idx + 1];

                    counter.Add(this._args[idx], this._args[idx + 1]);
                }
            }
        }

        private bool IsEndOfSentence(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return false;

            string endChar = word.Substring(word.Length - 1);

            return this._sentenceBoundaries.Contains(endChar);
        }
    }
}
