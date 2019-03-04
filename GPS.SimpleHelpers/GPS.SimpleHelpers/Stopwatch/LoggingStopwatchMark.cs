namespace GPS.SimpleHelpers.Stopwatch
{
    public class LoggingStopwatchMark
    {
        public string Mark {get;set;}
        public long ElapsedMilliseconds {get;set;}

        public override string ToString() =>
            $"{Mark}: {ElapsedMilliseconds}";
        
    }
}
