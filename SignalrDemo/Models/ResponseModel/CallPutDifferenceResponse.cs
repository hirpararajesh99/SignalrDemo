namespace SignalrDemo.Models.ResponseModel
{
    public class CallPutDifferenceResponse
    {
        public string Name { get; set; } = null!;
        public int? StrikePrice { get; set; }        
        public int? SumOfPut { get; set; }
        public int? SumOfCall { get; set; }
        public int? Difference { get; set; }
        public DateTime? ResponseDateTime { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
