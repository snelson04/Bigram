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
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Bigram.Test
{
    [TestFixture]
    public class FileParserTests
    {
        // fileName, expected bigram count, expected count of "the quick", crossSentenceBoundaries (t/f)
        [TestCase("ParseTest.txt", 7, 2, false)]   // 8 bigrams added but 7 values since 1 repeats
        [TestCase("Parse2LineTest.txt", 7, 4, false)]
        [TestCase("ParseTest.txt", 7, 2, true)]   // only 1 sentence so same as above
        [TestCase("Parse2LineTest.txt", 8, 4, true)] 
        public void TestSimpleFileParser(string filename, long expectedCount, long expectedRepeat, bool xsb)
        {

            IParser parser = new SimpleFileParser(filename, xsb);
            ICounter testCounter = new MemoryCounter();

            parser.Parse(testCounter);

            List<BigramCountValue> counts = testCounter.BigramCountList();
            Assert.AreEqual(expectedCount, counts.Count);   

            var count = from c in counts
                        where c.Bigram == "the quick"
                        select c.Count;

            Assert.AreEqual(1, count.Count());
            Assert.AreEqual(expectedRepeat, count.First());
        }
    }
}
