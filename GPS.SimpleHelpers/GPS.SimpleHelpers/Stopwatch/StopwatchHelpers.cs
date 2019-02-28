using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SW = System.Diagnostics.Stopwatch;

namespace GPS.SimpleHelpers.Stopwatch
{
    public static class StopwatchHelpers
    {
        public static TReturn TimeAction<TData, TReturn>(
            TData value, Func<TData, TReturn> func, Action<long> onFinish)
        {
            var sw = new SW();
            sw.Start();

            var result = func(value);

            onFinish(sw.ElapsedMilliseconds);

            sw.Stop();

            return result;
        }

        public static void TimeAction<TData>(
            TData value, Action<TData> action, Action<long> onFinish)
        {
            var sw = new SW();

            sw.Start();

            action(value);

            onFinish(sw.ElapsedMilliseconds);

            sw.Stop();
        }

        public static void TimeAction(Action action, Action<long> onFinish)
        {
            var sw = new SW();

            sw.Start();

            action();

            onFinish(sw.ElapsedMilliseconds);

            sw.Stop();

        }

        public static long TimeAction(Action action)
        {
            var sw = new SW();

            sw.Start();

            action();

            var result = sw.ElapsedMilliseconds;

            sw.Stop();

            return result;
        }

        public static long TimeAction<TData>(TData value, Action<TData> action)
        {
            var sw = new SW();

            sw.Start();

            action(value);

            var result = sw.ElapsedMilliseconds;

            sw.Stop();

            return result;
        }
    }

    public class LoggingStopwatch : SW
    {        
        private ConcurrentDictionary<Guid, LoggingStopwatchMark> _marks;

        public (long elapsedMilliseconds, Task<TOut> callbackResult) 
            Mark<TIn, TOut>(string markName, Func<TIn, Task<TOut>> callback = null, TIn callbackData = default(TIn))
        {
            var id = Guid.NewGuid();

            var elapsed = ElapsedMilliseconds;

            if(!IsRunning) 
            {
                Reset();
                Start();
                elapsed = 0;
            }
            
            _marks.TryAdd(id, new LoggingStopwatchMark { Mark = markName, ElapsedMilliseconds = elapsed});

            if(callback != null) return (elapsed, callback(callbackData));

            return (elapsed, null);
        }

        public double Difference(string firstMark, string secondMark) =>  
            _marks.Values.FirstOrDefault(v => v.Mark == secondMark).ElapsedMilliseconds -
            _marks.Values.FirstOrDefault(v => v.Mark == firstMark).ElapsedMilliseconds;
        
    }

    public class LoggingStopwatchMark
    {
        public string Mark {get;set;}
        public double ElapsedMilliseconds {get;set;}
    }
}
