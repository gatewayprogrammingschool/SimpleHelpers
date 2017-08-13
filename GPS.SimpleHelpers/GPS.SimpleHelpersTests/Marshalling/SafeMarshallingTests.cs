using Microsoft.VisualStudio.TestTools.UnitTesting;
using GPS.SimpleHelpers.Marshalling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS.SimpleHelpers.Marshalling.Tests
{
    [TestClass()]
    public class SafeMarshallingTests
    {
        private const string HELLO_WORLD = "Hello, World";
        private const string OOPS = "Oops";

        [TestMethod()]
        public void GetOrBuildSuccessTest()
        {            
            var result = SafeMarshalling.GetOrBuild<string>(
                () => HELLO_WORLD,
                () => OOPS);

            Assert.AreEqual(HELLO_WORLD, result);
        }

        [TestMethod()]
        public void GetOrBuildFailTest()
        {
            var result = SafeMarshalling.GetOrBuild<string>(
                () => null as string,
                () => OOPS);

            Assert.AreEqual(OOPS, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetOrBuildNullActionTest()
        {
            SafeMarshalling.GetOrBuild<string>(null, () => OOPS);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetOrBuildNullOnNullTest()
        {
            SafeMarshalling.GetOrBuild<string>(() => HELLO_WORLD, null);
        }

        [TestMethod()]
        public void TrySetSuccessTest()
        {
            var output = OOPS;
            var result = SafeMarshalling.TrySet<string>(HELLO_WORLD, s =>
            {
                output = s ?? throw new ArgumentNullException(nameof(s));
            });

            Assert.AreEqual(true, result);
            Assert.AreEqual(HELLO_WORLD, output);
        }

        [TestMethod()]
        public void TrySetFailTest()
        {
            var output = OOPS;
            var result = SafeMarshalling.TrySet<string>(null as string, s =>
            {
                output = s ?? throw new ArgumentNullException(nameof(s));
            });

            Assert.AreEqual(false, result);
            Assert.AreEqual(OOPS, output);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TrySetNullActionTest()
        {
            SafeMarshalling.TrySet<string>(null, null);
        }

        [TestMethod]
        public void SafeMarshallingCombinedTest()
        {
            var tc = new TestClass();

            var result = SafeMarshalling.GetOrBuild<string>(
                () => tc.Value, () => OOPS);

            Assert.AreEqual(OOPS, result);

            if (SafeMarshalling.TrySet<string>(HELLO_WORLD,
                value => tc.Value = value))
            {
                result = SafeMarshalling.GetOrBuild<string>(
                    () => tc.Value,
                    () => OOPS);

                Assert.AreEqual(HELLO_WORLD, result);
            }
        }
    }

    internal class TestClass
    {
        public string Value;
    }
}