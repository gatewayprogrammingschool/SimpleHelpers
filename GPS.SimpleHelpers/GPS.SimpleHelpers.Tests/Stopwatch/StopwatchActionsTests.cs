using GPS.SimpleHelpers.Stopwatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace GPS.SimpleHelpers.Stopwatch.Tests
{
    public class StopwatchActionsTests
    {
        ITestOutputHelper _log;

        public StopwatchActionsTests(ITestOutputHelper log)
        {
            _log = log;
        }

        private async Task RandomDelay(int start, int end)
        {
                var random = new Random();
                await Task.Delay((int)(random.NextDouble() * (double)random.Next(start, end)));
        }

        [Fact]
        public async Task TimeActionTest()
        {
            var result = await StopwatchHelpers.TimeAction<object, object>(null, async value =>
            {
                await RandomDelay(15, 150);
                return value;
            },
            elapsed =>
            {
                _log.WriteLine($"{elapsed} ms");
            });

            Assert.Null(result);
        }

        [Fact]
        public void TimeActionTest1()
        {
            var result = false;
            StopwatchHelpers.TimeAction(() =>
            {
                _log.WriteLine("Action called.");
            },
            elapsed =>
            {
                _log.WriteLine($"{elapsed} ms");
                result = true;
            });

            Assert.True(result);
        }

        [Fact]
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
                        _log.WriteLine($". [{elapsed} ms]");
                    });
            }

            Assert.True(result);
        }

        [Fact]
        public void TimeActionTest2()
        {
            var elapsed = StopwatchHelpers.TimeAction(() =>
                { for (int i = 0; i < Math.Pow(2, 18); i++) ; });

            Assert.NotEqual(0, elapsed);
        }

        [Fact]
        public void TimeActionTest3()
        {
            for (int i = 0; i < 10; ++i)
            {
                Assert.NotEqual(0, StopwatchHelpers.TimeAction(i, j =>
                    { for (int k = 0; k < Math.Pow(2, j + 16); ++k) ; }));
            }
        }
    }
}