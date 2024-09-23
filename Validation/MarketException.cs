using System;

namespace Business.Validation
{
    public class MarketException : Exception
    {
        public MarketException() : base("An error occurred in the Trade Market system.")
        {

        }
        public MarketException(string message) : base(message)
        {
        }
        public MarketException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
