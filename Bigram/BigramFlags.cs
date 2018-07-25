using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAE.CLI.Flags;

namespace Bigram
{
    public class BigramFlags : Parser
    {
        public String Filepath
        {
            get
            {
                FlagValue<string> fv = this["file"] as FlagValue<string>;
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

        public BigramFlags(string[] args)
        {
            this.AddStringFlag("filepath", "", "The name (with path) of the file to process.");
            this.AddBoolFlag("time", false, "flag to indicate whether to time the parsing and counting of the bigrams");
            this.AddStringFlag("order", "none", "Order the output by: None (default), Frequency of count (-order=freq), or Alphabetically (-order=alpha)");

            this.Parse(args);
        }
    }
}
