using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;

using Bigram.Core;


namespace Bigram.Test
{
    [TestFixture]
    public class CoreTests
    {
        [Test]
        public void TestMemoryCounter()
        {
            ICounter testCounter = new MemoryCounter();

            //"The", "quick", "brown", "fox", "and", "the", "quick", "blue", "hare"
            testCounter.Add("the", "quick");
            testCounter.Add("quick", "brown");
            testCounter.Add("brown", "fox");
            testCounter.Add("fox", "and");
            testCounter.Add("and", "the");
            testCounter.Add("the", "quick");
            testCounter.Add("quick", "blue");
            testCounter.Add("blue", "hare");

            List<BigramCountValue> counts = testCounter.BigramCountList();
            Assert.AreEqual(7, counts.Count);   // 8 bigrams added but 7 values since 1 repeats

            var count = from c in counts
                         where c.Bigram == "the quick"
                        select c.Count;

            Assert.AreEqual(1, count.Count());
            Assert.AreEqual(2, count.First());
        }

        [Test]
        public void TestSimpleFileParser()
        {

            IParser parser = new SimpleFileParser("ParseTest.txt");

            ICounter testCounter = new MemoryCounter();

            parser.Parse(testCounter);

            List<BigramCountValue> counts = testCounter.BigramCountList();
            Assert.AreEqual(7, counts.Count);   // 8 bigrams added but 7 values since 1 repeats

            var count = from c in counts
                        where c.Bigram == "the quick"
                        select c.Count;

            Assert.AreEqual(1, count.Count());
            Assert.AreEqual(2, count.First());
        }

        [Test]
        public void TestCommandLineArgsParser()
        {
            string[] args = new string[] { "The", "quick", "brown", "fox", "and", "the", "quick", "blue", "hare" };
            IParser parser = new CommandLineArgsParser(args);

            ICounter testCounter = new MemoryCounter();

            parser.Parse(testCounter);

            List<BigramCountValue> counts = testCounter.BigramCountList();
            Assert.AreEqual(7, counts.Count);   // 8 bigrams added but 7 values since 1 repeats

            var count = from c in counts
                        where c.Bigram == "the quick"
                        select c.Count;

            Assert.AreEqual(1, count.Count());
            Assert.AreEqual(2, count.First());
        }
    }
}
