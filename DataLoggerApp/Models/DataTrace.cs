namespace DataLoggerApp.Models
{

    public class DataTrace
    {
        public Method Method { get; set; }
        public Process Process { get; set; }
        public string Layer { get; set; }
        public Creation Creation { get; set; }
        public string Type { get; set; }
        public bool IsProcessed { get; set; }
    }
}
