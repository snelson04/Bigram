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

namespace Bigram.Test
{
    [TestFixture]
    public class FactoryTest
    {
        [Test]
        public void TestFactorySingleton()
        {
            IFactory factory = Factory.GetFactory();
            Assert.IsNotNull(factory);

            Assert.AreSame(factory, Factory.GetFactory());
        }

        [Test]
        public void TestCreateFileParser()
        {
            BigramFlags flags = new BigramFlags(new string[] { "-filepath", "ParseTest.txt" });
            IParser parser = Factory.GetFactory().CreateParser(flags);

            Assert.IsNotNull(parser as SimpleFileParser);
        }

        [Test]
        public void TestCreateCommandLineParser()
        {
            BigramFlags flags = new BigramFlags(new string[] { "The", "quick", "brown", "fox", "and", "the", "quick", "blue", "hare" });
            IParser parser = Factory.GetFactory().CreateParser(flags);

            Assert.IsNotNull(parser as CommandLineTextParser);
        }

        [Test]
        public void TestCreateMemoryCounter()
        {
            BigramFlags flags = new BigramFlags(new string[] { "-filepath", "ParseTest.txt" });            
            ICounter counter = Factory.GetFactory().CreateCounter(flags);

            Assert.IsNotNull(counter as MemoryCounter);
        }

    }
}
