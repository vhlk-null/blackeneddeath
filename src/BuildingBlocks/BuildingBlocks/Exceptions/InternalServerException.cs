namespace BuildingBlocks.Exceptions
{
    public class InternalServerException : Exception
    {
        public InternalServerException(string message) : base(message)
        {            
        }

        public InternalServerException(string messge, string detailes) : base(messge)
        {
            Details = detailes;
        }

        public string? Details { get; set; }
    }
}
