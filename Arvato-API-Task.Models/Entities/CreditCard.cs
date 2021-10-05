using System;

namespace Arvato_API_Task.Models.Entities
{
    public class CreditCard
    {
        public string Owner { get; set; }
        public long Number { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CVV { get; set; }
    }

    public enum CCSystem
    {
        UNKNOWN,
        AMERICAN_EXPRESS,
        VISA,
        MASTER_CARD,
    }
}
