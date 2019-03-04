using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using SW = System.Diagnostics.Stopwatch;

namespace GPS.SimpleHelpers.Stopwatch
{
    public class LoggingStopwatch : SW
    {
        private ConcurrentDictionary<Guid, ElapsedMark> _elapsedMarks =
            new ConcurrentDictionary<Guid, ElapsedMark>();

        private ConcurrentDictionary<Guid, ExecuteMark> _executionMarks =
            new ConcurrentDictionary<Guid, ExecuteMark>();

        public (long elapsedMilliseconds, long executionMilliseconds)
            Mark(string markName, Action callback = null)
        {
            var result = Mark<object, object>(markName, new Func<object, object>(o =>
            {
                callback?.Invoke();
                return null;
            }));

            return (result.elapsedMilliseconds, result.executionMilliseconds);
        }

        public (long elapsedMilliseconds, long executionMilliseconds)
            Mark<TIn>(string markName,
            Action<TIn> callback = null,
            TIn callbackData = default(TIn))
        {
            var result = Mark<TIn, object>(markName, new Func<TIn, object>(o =>
            {
                callback?.Invoke(callbackData);
                return null;
            }), callbackData);

            return (result.elapsedMilliseconds, result.executionMilliseconds);
        }

        public (long elapsedMilliseconds, long executionMilliseconds, TOut callbackResult)
            Mark<TIn, TOut>(string markName,
                Func<TIn, TOut> callback = null,
                TIn callbackData = default(TIn))
        {
            var id = Guid.NewGuid();

            var start = ElapsedMilliseconds;
            _elapsedMarks.TryAdd(Guid.NewGuid(), new ElapsedMark { Mark = $"{markName}_Begin", ElapsedMilliseconds = start });

            if (!IsRunning)
            {
                Reset();
                Start();
                start = 0;
            }

            TOut result = default(TOut);

            if (callback != null)
            {
                result = callback(callbackData);
            }

            var end = ElapsedMilliseconds;

            _elapsedMarks.TryAdd(Guid.NewGuid(), new ElapsedMark { Mark = $"{markName}_End", ElapsedMilliseconds = end });

            var executionMilliseconds = end - start;

            _executionMarks.TryAdd(id, new ExecuteMark { Mark = markName, Start = start, End = end });

            return (end, executionMilliseconds, result);
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

            _elapsedMarks.TryAdd(id, new ElapsedMark
            {
                Mark = markName,
                ElapsedMilliseconds = elapsed
            });

            return elapsed;
        }
        public long Difference(string firstMark, string secondMark) =>
            _elapsedMarks.Values.FirstOrDefault(v => v.Mark == secondMark)?.ElapsedMilliseconds ?? 0 -
            _elapsedMarks.Values.FirstOrDefault(v => v.Mark == firstMark)?.ElapsedMilliseconds ?? 0;

        public long ElapsedAt(string mark) =>
            _elapsedMarks.Values.FirstOrDefault(v => v.Mark == mark)?.ElapsedMilliseconds ?? 0;

        public IOrderedEnumerable<ElapsedMark> ElapsedMarks =>
            _elapsedMarks.Values.OrderBy(m => m.ElapsedMilliseconds);

        public IOrderedEnumerable<ExecuteMark> ExecutionMarks =>
            _executionMarks.Values.OrderBy(m => m.Start);

        public void Clear()
        {
            _elapsedMarks.Clear();
        }

        public new void Reset()
        {
            Clear();
            base.Reset();
        }
    }
}
