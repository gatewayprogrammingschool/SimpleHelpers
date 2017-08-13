using Microsoft.VisualStudio.TestTools.UnitTesting;
using GPS.SimpleHelpers.Stopwatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GPS.SimpleHelpers.Stopwatch.Tests
{
    [TestClass()]
    public class StopwatchHelpersTests
    {
        [TestMethod()]
        public void TimeActionTest()
        {
            var result = StopwatchHelpers.TimeAction<int, bool>(10, value =>
            {
                return value == 10;
            },
            elapsed =>
            {
                System.Diagnostics.Debug.WriteLine($"{elapsed} ms");
            });

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TimeActionTest1()
        {
            var result = false;
            StopwatchHelpers.TimeAction(() =>
            {
                System.Diagnostics.Debug.WriteLine("Action called.");
            },
            elapsed =>
            {
                System.Diagnostics.Debug.WriteLine($"{elapsed} ms");
                result = true;
            });

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TimeActionComplexTest()
        {
            var result = false;
            var values = new[] { "1", "2", "three", "4", "5", "six" };

            foreach (var value in values)
            {
                StopwatchHelpers.TimeAction<string>(value,
                    val =>
                    {
                        int v;
                        if (Int32.TryParse(val, out v))
                        {
                            for (int i = 0; i < v * 1024; ++i)
                            {
                                if (i % 128 == 0)
                                {
                                    System.Diagnostics.Debug.Write($".");
                                    Thread.Sleep(10);
                                }
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.Write($". {val} ");
                        }
                    },
                    elapsed =>
                    {
                        result |= true;
                        System.Diagnostics.Debug.WriteLine($". [{elapsed} ms]");
                    });
            }

            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TimeActionTest2()
        {
            var elapsed = StopwatchHelpers.TimeAction(() =>
                { for (int i = 0; i < Math.Pow(2, 18); i++) ; });

            Assert.AreNotSame(0, elapsed);
        }

        [TestMethod()]
        public void TimeActionTest3()
        {
            for(int i = 0; i < 10; ++i)
            {
                Assert.AreNotSame(0, StopwatchHelpers.TimeAction(i, j => 
                    { for (int k = 0; k < Math.Pow(2, j + 16); ++k) ; }));
            }
        }
    }
}