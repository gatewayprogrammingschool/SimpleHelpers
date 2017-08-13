using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS.SimpleHelpers.Marshalling
{
    public static class SafeMarshalling
    {
        public static TResult GetOrBuild<TResult>(Func<TResult> getter, Func<TResult> builder)
            where TResult: class
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter), "Must supply getter.");
            if (builder == null) throw new ArgumentNullException(nameof(builder), "Must supply onNull.");

            var result = getter();

            if (result == null) result = builder();

            return result;
        }

        public static bool TrySet<TData>(TData value, Action<TData> setter)
        {
            if (setter == null) throw new ArgumentNullException(nameof(setter), "Must supply setter.");

            var exception = SafeCall.SafeCallHelpers.TryCall(() => setter(value));

            return exception == null;
        }
    }
}
