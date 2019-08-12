using GPS.SimpleHelpers.SafeCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GPS.SimpleHelpers.SafeCall.Tests
{
    public class SafeCallHelpersTests
    {
        ITestOutputHelper _log;

        public SafeCallHelpersTests(ITestOutputHelper log)
        {
            _log = log;
        }
        
        [Fact]
        public void TryCallTest()
        {
            var result = SafeCallHelpers.TryCall(
                () => throw new ApplicationException("Test"), 
                () => { });

            Assert.IsType<ApplicationException>(result);
        }

        [Fact]
        public void TryCallNoExceptionTest()
        {
            var a = 1;
            var b = 2;
            double c = 0.0;
            double d = -1.0;

            var result = SafeCallHelpers.TryCall(
                () => { c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)); },
                () => { d = c; });

            Assert.Equal(Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)), c);
            Assert.Equal(c, d);
        }

        [Fact]
        public void TryCallNoFinallyTest()
        {
            var result = SafeCallHelpers.TryCall(() => throw new ApplicationException("Test"));

            Assert.IsType<ApplicationException>(result);
        }

        [Fact]
        public void TryCallGenericExceptionWithFinallyTest()
        {
            Assert.Throws<ApplicationException>(() =>
                SafeCallHelpers.TryCall<ArgumentException>(
                    () => throw new ApplicationException("Test"),
                    () => _log.WriteLine("Finally called.")));
        }

        [Fact]
        public void TryCallGenericExceptionTest()
        {
            Assert.Throws<ApplicationException>(() =>
                SafeCallHelpers.TryCall<ArgumentNullException>(
                    () => throw new ApplicationException("Test")));
        }
    }
}