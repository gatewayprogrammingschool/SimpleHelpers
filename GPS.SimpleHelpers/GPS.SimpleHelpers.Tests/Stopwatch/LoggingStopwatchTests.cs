using Xunit;
using Xunit.Abstractions;
using GPS.SimpleHelpers.Stopwatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GPS.SimpleHelpers.Stopwatch.Tests
{
    public class LoggingStopwatchTests
    {
        ITestOutputHelper _log;

        public LoggingStopwatchTests(ITestOutputHelper log)
        {
            _log = log;
        }

        [Fact]
        public void FirstMarkIsZeroElapsed()
        {
            var lsw = new LoggingStopwatch();

            var output = lsw.Mark<LoggingStopwatch, long>("first", 
            (sw) =>
            {
                var start = sw.ElapsedAt("first");
                _log.WriteLine(start.ToString("N4"));

                Assert.Equal(0.0, start);

                return start;
            }, lsw);
        }

        [Fact]
        public void SecondIsLongerThanFirst()
        {
            var lsw = new LoggingStopwatch();

            lsw.Mark("first");

            long markElapsed = lsw.Mark("second");
            long difference = lsw.Difference("first", "second");

            _log.WriteLine($"Mark At: {markElapsed:N4}");
            _log.WriteLine($"Difference: {difference:N4}");

            Assert.NotEqual(0.0, markElapsed);
            Assert.NotEqual(0.0, difference);
        }

        [Fact]
        public async Task MarksListMatchesValues()
        {
            var random = new Random();
            var lsw = new LoggingStopwatch();

            var first = lsw.Mark("first");
            await Task.Delay((int)(random.NextDouble() * (double)random.Next(15, 150)));
            var second = lsw.Mark("second");
            await Task.Delay((int)(random.NextDouble() * (double)random.Next(15, 150)));
            var third = lsw.Mark("third");

            var marks = lsw.Marks;

            _log.WriteLine($"Marks: {string.Join(", ", marks)}");

            Assert.Equal(lsw.Difference("first", "third"),
                marks.First(m => m.Mark == "third").ElapsedMilliseconds -
                marks.First(m => m.Mark == "first").ElapsedMilliseconds);
        }
    }
}