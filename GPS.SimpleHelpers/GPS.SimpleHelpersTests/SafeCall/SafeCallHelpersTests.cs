using Microsoft.VisualStudio.TestTools.UnitTesting;
using GPS.SimpleHelpers.SafeCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS.SimpleHelpers.SafeCall.Tests
{
    [TestClass()]
    public class SafeCallHelpersTests
    {
        [TestMethod()]
        public void TryCallTest()
        {
            var result = SafeCallHelpers.TryCall(
                () => throw new ApplicationException("Test"), 
                () => { });

            Assert.IsInstanceOfType(result, typeof(ApplicationException));
        }

        [TestMethod()]
        public void TryCallNoExceptionTest()
        {
            var a = 1;
            var b = 2;
            double c = 0.0;
            double d = -1.0;

            var result = SafeCallHelpers.TryCall(
                () => { c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)); },
                () => { d = c; });

            Assert.AreEqual(Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)), c);
            Assert.AreEqual(c, d);
        }

        [TestMethod()]
        public void TryCallNoFinallyTest()
        {
            var result = SafeCallHelpers.TryCall(() => throw new ApplicationException("Test"));

            Assert.IsInstanceOfType(result, typeof(ApplicationException));
        }

        [TestMethod()]
        [ExpectedException(typeof(ApplicationException))]
        public void TryCallGenericExceptionWithFinallyTest()
        {
            var result = SafeCallHelpers.TryCall<ArgumentNullException>(
                () => throw new ApplicationException("Test"),
                () => System.Diagnostics.Debug.WriteLine("Finally called."));
        }

        [TestMethod()]
        [ExpectedException(typeof(ApplicationException))]
        public void TryCallGenericExceptionTest()
        {
            var result = SafeCallHelpers.TryCall<ArgumentNullException>(
                () => throw new ApplicationException("Test"));
        }
    }
}