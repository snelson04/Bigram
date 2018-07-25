using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bigram.Core
{
    public class BigramCountValue : Tuple<string, long>
    {
        public string Bigram { get { return this.Item1; } }
        public long Count { get { return this.Item2; } }

        public BigramCountValue(string item1, long item2) : base(item1, item2)
        {
        }
    }
}
