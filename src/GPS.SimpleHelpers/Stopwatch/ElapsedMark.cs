namespace GPS.SimpleHelpers.Stopwatch
{
    public class ElapsedMark
    {
        public string Mark {get;set;}
        public long ElapsedMilliseconds {get;set;}

        public override string ToString() =>
            $"{Mark}: {ElapsedMilliseconds}";
    }
}
