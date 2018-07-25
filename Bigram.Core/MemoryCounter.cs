using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bigram.Core
{
    public class MemoryCounter : ICounter
    {
        private Dictionary<String, Dictionary<String, long>> _BigramMap;

        /// <summary>
        /// A 2 level Dictionary that maps the first word to a dictionary that contains the second word mapped to the count
        ///  For small files a single level dictionary with both words in one string would suffice but would likely become a bottleneck
        ///  with larger files.  Given a large enough file the same will happen with the 2 level store and would probably best be handled 
        ///  in a database.
        ///  
        /// </summary>
        public MemoryCounter()
        {
            this._BigramMap = new Dictionary<string, Dictionary<string, long>>();
        }

        /// <summary>
        /// Stores the bigram represented in the 2 words and counts increments a counter each time it is added.
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns></returns>
        public long Add(string word1, string word2)
        {
            word1 = word1.ToLower();
            word2 = word2.ToLower();

            long currentCount = 0;
            Dictionary<string, long> firstWordMap = null;
            firstWordMap = FindFirstWord(word1);

            if (firstWordMap.ContainsKey(word2))
                currentCount = 1 + firstWordMap[word2];
            else
                currentCount = 1;

            firstWordMap[word2] = currentCount;

            return currentCount;
        }

        /// <summary>
        /// Returns the list of BigramCountValues that have been processed by the counter
        /// </summary>
        /// <returns></returns>
        public List<BigramCountValue> BigramCountList()
        {
            List<BigramCountValue> bigramCounts = new List<BigramCountValue>();

            foreach (var first in this._BigramMap)
            {
                foreach (var second in first.Value)
                    bigramCounts.Add(new BigramCountValue(String.Format("{0} {1}", first.Key, second.Key), second.Value));
            }

            return bigramCounts;
        }

        private Dictionary<string, long> FindFirstWord(string word1)
        {
            Dictionary<string, long> map;

            if (this._BigramMap.ContainsKey(word1))
                map = this._BigramMap[word1];
            else
            {
                map = new Dictionary<string, long>();
                this._BigramMap[word1] = map;
            }

            return map;
        }
    }
}
