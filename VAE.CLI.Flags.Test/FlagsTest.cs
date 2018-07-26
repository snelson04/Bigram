/*
Copyright 2015 VAE, Inc.

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
using System.Collections.Generic;

using NUnit.Framework;

using VAE.CLI.Flags;

namespace VAE.CLI.Tests
{
    [TestFixture]
    public class FlagsTest
    {
        [Test]
        public void TestFlagParsing()
        {
            string[] args = new string[] {"--mystringflag", "test.txt", "-myintflag=10", "-myboolFlag" };
            var flags = new Parser();
            
            var intFlag = flags.AddIntFlag("myintflag", 0, "An integer flag"); // defaults to 0
            var stringFlag = flags.AddStringFlag("mystringflag", "nothing", "A string flag"); // defaults to "nothing"
            var boolFlag = flags.AddBoolFlag("myboolFlag", false, "A bool flag"); // defaults to false.
            
            flags.Parse(args);

            Assert.IsFalse(flags.HasErrors);
            FlagValue<string> flagString = flags["mystringflag"] as FlagValue<string>;
            Assert.AreEqual("test.txt", flagString.Value);

            FlagValue<bool> flagBool = flags["myboolFlag"] as FlagValue<bool>;
            Assert.AreEqual(true, flagBool.Value);

            FlagValue<int> flagInt = flags["myintflag"] as FlagValue<int>;
            Assert.AreEqual(10, flagInt.Value);

        }

        [Test]
        public void TestFlagParsingError()
        {
            string[] args = new string[] { "-myintflag=", "--mystringflag", "test.txt", "-myintflag=10", "-myboolFlag" };
            var flags = new Parser();

            var intFlag = flags.AddIntFlag("myintflag", 0, "An integer flag"); // defaults to 0
            var stringFlag = flags.AddStringFlag("mystringflag", "nothing", "A string flag"); // defaults to "nothing"
            var boolFlag = flags.AddBoolFlag("myboolFlag", false, "A bool flag"); // defaults to false.

            flags.Parse(args);

            Assert.IsTrue(flags.HasErrors);

            // The parser breaks once an error is enountered so these values would NOT be set
            FlagValue<string> flagString = flags["mystringflag"] as FlagValue<string>;
            Assert.AreNotEqual("test.txt", flagString.Value);

            FlagValue<bool> flagBool = flags["myboolFlag"] as FlagValue<bool>;
            Assert.AreNotEqual(true, flagBool.Value);

            FlagValue<int> flagInt = flags["myintflag"] as FlagValue<int>;
            Assert.AreNotEqual(10, flagInt.Value);

        }

    }
}

