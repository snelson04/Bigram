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
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bigram.Test
{
    [TestFixture]
    public class CoreTests
    {
        [TestCase("the quick", 2)]
        [TestCase("brown fox", 1)]
        public void TestMemoryCounter(string bigram, long expectedCount)
        {
            ICounter testCounter = new MemoryCounter();

            testCounter.Add("the", "quick");
            testCounter.Add("quick", "brown");
            testCounter.Add("brown", "fox");
            testCounter.Add("fox", "and");
            testCounter.Add("and", "the");
            testCounter.Add("the", "quick");
            testCounter.Add("quick", "blue");
            testCounter.Add("blue", "hare");

            List<BigramCountValue> bigramCountList = testCounter.BigramCountList();
            Assert.AreEqual(7, bigramCountList.Count);   // 8 bigrams added but 7 values since 1 repeats

            var targetBigramCounts = from c in bigramCountList
                                        where c.Bigram == bigram
                                        select c.Count;

            Assert.AreEqual(1, targetBigramCounts.Count()); // there should be only one instance of this bigram count
            Assert.AreEqual(expectedCount, targetBigramCounts.First());
        }


        [TestCase("The quick brown fox and the quick blue hare.", 2)]
        [TestCase("The quick brown fox and the quick blue hare. The quick brown fox and the quick blue hare.", 4)]
        public void TestCommandLineArgsParser(string argString, long expectedValue)
        {
            string[] args = argString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            IParser parser = new CommandLineTextParser(args);

            ICounter testCounter = new MemoryCounter();

            parser.Parse(testCounter);

            List<BigramCountValue> counts = testCounter.BigramCountList();
            Assert.AreEqual(7, counts.Count);   // 8 bigrams added but 7 values since 1 repeats

            var count = from c in counts
                        where c.Bigram == "the quick"
                        select c.Count;

            Assert.AreEqual(1, count.Count());
            Assert.AreEqual(expectedValue, count.First());
        }
    }
}
