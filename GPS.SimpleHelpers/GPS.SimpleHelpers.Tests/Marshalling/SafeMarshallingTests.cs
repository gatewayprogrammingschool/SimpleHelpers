using GPS.SimpleHelpers.Marshalling;
using Xunit;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS.SimpleHelpers.Marshalling.Tests
{
    
    public class SafeMarshallingTests
    {
        ITestOutputHelper _log;

        public SafeMarshallingTests(ITestOutputHelper log)
        {
            _log = log;
        }
        
        private const string HELLO_WORLD = "Hello, World";
        private const string OOPS = "Oops";

        [Fact]
        public void GetOrBuildSuccessTest()
        {            
            var result = SafeMarshalling.GetOrBuild<string>(
                () => HELLO_WORLD,
                () => OOPS);

            Assert.Equal(HELLO_WORLD, result);
        }

        [Fact]
        public void GetOrBuildFailTest()
        {
            var result = SafeMarshalling.GetOrBuild<string>(
                () => null as string,
                () => OOPS);

            Assert.Equal(OOPS, result);
        }

        [Fact]
        public void GetOrBuildNullActionTest()
        {
            Assert.Throws<ArgumentNullException>(() => SafeMarshalling.GetOrBuild<string>(null, () => OOPS));
        }

        [Fact]
        public void GetOrBuildNullOnNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => SafeMarshalling.GetOrBuild<string>(null, null));
        }

        [Fact]
        public void TrySetSuccessTest()
        {
            var output = OOPS;
            var result = SafeMarshalling.TrySet<string>(HELLO_WORLD, s =>
            {
                output = s ?? throw new ArgumentNullException(nameof(s));
            });

            Assert.True(result);
            Assert.Equal(HELLO_WORLD, output);
        }

        [Fact]
        public void TrySetFailTest()
        {
            var output = OOPS;
            var result = SafeMarshalling.TrySet<string>(null as string, s =>
            {
                output = s ?? throw new ArgumentNullException(nameof(s));
            });

            Assert.False(result);
            Assert.Equal(OOPS, output);
        }

        [Fact]
        public void TrySetNullActionTest()
        {
            Assert.Throws<ArgumentNullException>(() => SafeMarshalling.GetOrBuild<string>(null, null));
        }

        [Fact]
        public void SafeMarshallingCombinedTest()
        {
            var tc = new TestClass();

            var result = SafeMarshalling.GetOrBuild<string>(
                () => tc.Value, () => OOPS);

            Assert.Equal(OOPS, result);

            if (SafeMarshalling.TrySet<string>(HELLO_WORLD,
                value => tc.Value = value))
            {
                result = SafeMarshalling.GetOrBuild<string>(
                    () => tc.Value,
                    () => OOPS);

                Assert.Equal(HELLO_WORLD, result);
            }
        }
    }

    internal class TestClass
    {
        public string Value;
    }
}