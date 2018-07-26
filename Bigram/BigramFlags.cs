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
using VAE.CLI.Flags;

namespace Bigram
{
    public class BigramFlags : Parser
    {
        public String Filepath
        {
            get
            {
                FlagValue<string> fv = this["filepath"] as FlagValue<string>;
                return fv.Value;
            }
        }

        public String Order
        {
            get
            {
                FlagValue<string> fv = this["order"] as FlagValue<string>;
                return fv.Value;
            }
        }

        public bool Time
        {
            get
            {
                FlagValue < bool > fv = this["time"] as FlagValue<bool>;
                return fv.Value;
            }
        }

        public bool CrossSentenceBoundaries
        {
            get
            {
                FlagValue<bool> fv = this["xsb"] as FlagValue<bool>;
                return fv.Value;
            }
        }

        public BigramFlags(string[] args)
        {
            this.AddStringFlag("filepath", "", "The name (with path) of the file to process.");
            this.AddBoolFlag("time", false, "flag to indicate whether to time the parsing and counting of the bigrams");
            this.AddBoolFlag("xsb", false, "flag to indicate whether bigrams can cross sentence boundaries [defaults to false]");
            this.AddStringFlag("order", "none", "Order the output by: None (default), Frequency of count (-order=freq (ascending) or =freq_d (descending)), or Alphabetically (-order=alpha)");

            this.Parse(args);
        }
    }
}
