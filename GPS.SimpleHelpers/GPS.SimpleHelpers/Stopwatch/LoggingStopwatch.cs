using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using SW = System.Diagnostics.Stopwatch;

namespace GPS.SimpleHelpers.Stopwatch
{
    public class LoggingStopwatch : SW
    {
        private ConcurrentDictionary<Guid, LoggingStopwatchMark> _marks =
            new ConcurrentDictionary<Guid, LoggingStopwatchMark>();

        public (long elapsedMilliseconds, TOut callbackResult)
            Mark<TIn, TOut>(string markName, 
                Func<TIn, TOut> callback = null, 
                TIn callbackData = default(TIn))
        {
            var id = Guid.NewGuid();

            var elapsed = ElapsedMilliseconds;

            if (!IsRunning)
            {
                Reset();
                Start();
                elapsed = 0;
            }

            _marks.TryAdd(id, new LoggingStopwatchMark { Mark = markName, ElapsedMilliseconds = elapsed });

            if (callback != null) return (elapsed, callback(callbackData));

            return (elapsed, default(TOut));
        }

        public long Mark(string markName)
        {
            var id = Guid.NewGuid();

            var elapsed = ElapsedMilliseconds;

            if (!IsRunning)
            {
                Reset();
                Start();
                elapsed = 0;
            }

            _marks.TryAdd(id, new LoggingStopwatchMark
            {
                Mark = markName,
                ElapsedMilliseconds = elapsed
            });

            return elapsed;
        }
        public long Difference(string firstMark, string secondMark) =>
            _marks.Values.FirstOrDefault(v => v.Mark == secondMark).ElapsedMilliseconds -
            _marks.Values.FirstOrDefault(v => v.Mark == firstMark).ElapsedMilliseconds;

        public long ElapsedAt(string mark) =>        
            _marks.Values.FirstOrDefault(v => v.Mark == mark).ElapsedMilliseconds;   

        public IOrderedEnumerable<LoggingStopwatchMark> Marks =>
            _marks.Values.OrderBy(m => m.ElapsedMilliseconds);     
    }
}
