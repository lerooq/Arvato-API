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
        UNKNOWN = 0,
        AMERICAN_EXPRESS = 3, // ^3[47][0-9]{13}$
        VISA = 4, // ^4[0-9]{12}(?:[0-9]{3})?$
        MASTER_CARD = 5, // ^5[1-5][0-9]{14}$|^2(?:2(?:2[1-9]|[3-9][0-9])|[3-6][0-9][0-9]|7(?:[01][0-9]|20))[0-9]{12}$
    }

    /*
     * validateCard(cardOwner:string, cardNumber:string, cvc:string, expirationMonth:string, expirationYear:string) {
  let errors: string[] = [];
  let cleanCardNumber = cardNumber.replace("-", "");
  
  let cardType = cleanCardNumber[0];
  switch(cardType) {
    // Visa
    case "5":
      let match = cleanCardNumber.match(/^[\d]{16}$/i);
      if (!match) {
        errors.push("Invalid card number");
      }
      break;
    case "...": ...
  }

  let month = parseInt(expirationMonth);
  let year = parseInt(expirationYear);
  let expirationDate = new Date(year, month);
  if (Date.now() - expirationDate > 0) {
    errors.push('Card is expired')
  }

  // etc etc etc

  if (errors.length > 0) {
    return res.status(400).json({ errors });
  }

  return res.json({ cardType: Cards.getCardName(cardType) });
}*/
}
