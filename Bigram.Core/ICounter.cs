using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bigram.Core
{
    public interface ICounter
    {
        long Add(string word1, string word2);
        List<BigramCountValue> BigramCountList();
    }
}
