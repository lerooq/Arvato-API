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
        AMERICAN_EXPRESS, // ^3[47][0-9]{13}$
        VISA, // ^4[0-9]{12}(?:[0-9]{3})?$
        MASTER_CARD, // ^5[1-5][0-9]{14}$|^2(?:2(?:2[1-9]|[3-9][0-9])|[3-6][0-9][0-9]|7(?:[01][0-9]|20))[0-9]{12}$
    }
}
