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


namespace Bigram.Core
{
    public class CommandLineTextParser : BaseParser, IParser
    {
        private string[] _args;

        public CommandLineTextParser(string[] args, bool crossSentenceBoundaries = false) : base(crossSentenceBoundaries)
        {
            this._args = args;
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

    }
}
