using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS.SimpleHelpers.Stopwatch
{
    public static class StopwatchHelpers
    {
        public static TReturn TimeAction<TData, TReturn>(
            TData value, Func<TData, TReturn> func, Action<long> onFinish)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            var result = func(value);

            onFinish(sw.ElapsedMilliseconds);

            sw.Stop();

            return result;
        }

        public static void TimeAction<TData>(
            TData value, Action<TData> action, Action<long> onFinish)
        {
            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            action(value);

            onFinish(sw.ElapsedMilliseconds);

            sw.Stop();
        }

        public static void TimeAction(Action action, Action<long> onFinish)
        {
            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            action();

            onFinish(sw.ElapsedMilliseconds);

            sw.Stop();

        }

        public static long TimeAction(Action action)
        {
            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            action();

            var result = sw.ElapsedMilliseconds;

            sw.Stop();

            return result;
        }

        public static long TimeAction<TData>(TData value, Action<TData> action)
        {
            var sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            action(value);

            var result = sw.ElapsedMilliseconds;

            sw.Stop();

            return result;
        }
    }
}
