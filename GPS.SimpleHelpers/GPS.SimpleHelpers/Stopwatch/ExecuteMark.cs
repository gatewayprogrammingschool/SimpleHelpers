namespace GPS.SimpleHelpers.Stopwatch
{
    public class ExecuteMark {
        public string Mark {get;set;}
        public long Start {get;set;}
        public long End {get;set;}

        public long ExecutionMilliseconds => End - Start;

        public override string ToString() =>
            $"{Mark}: {ExecutionMilliseconds}";
    }
}
