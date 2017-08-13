using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS.SimpleHelpers.SafeCall
{
    public static class SafeCallHelpers
    {
        public static Exception TryCall(Action action, Action final)
        {
            var result = null as Exception;

            try { action(); }
            catch(Exception ex) { result = ex; }
            finally { final(); }

            return result;
        }

        public static Exception TryCall(Action action)
        {
            var result = null as Exception;

            try { action(); }
            catch (Exception ex) { result = ex; }

            return result;
        }

        public static Exception TryCall<TData>(TData value, Action<TData> action)
        {
            var result = null as Exception;

            try { action(value); }
            catch (Exception ex) { result = ex; }

            return result;
        }

        public static TException TryCall<TException>(Action action, Action final)
            where TException: Exception
        {
            var result = null as TException;

            try { action(); }
            catch (TException ex) { result = ex; }
            finally { final(); }

            return result;
        }

        public static TException TryCall<TException>(Action action)
            where TException : Exception
        {
            var result = null as TException;

            try { action(); }
            catch (TException ex) { result = ex; }

            return result;
        }
    }
}
