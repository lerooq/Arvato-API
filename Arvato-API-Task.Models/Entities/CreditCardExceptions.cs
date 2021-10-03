using System;

namespace Arvato_API_Task.Models.Entities
{
    // ApplicationExceptions could be used instead of returning non-descript booleans when validating credit card
    // This would allow for more cleaner / more specific error handling

    // generic
    public class CreditCardException : ApplicationException
    {
        public CreditCardException(string message) : base(message)
        {
        }
    }

    // all fields are provided
    public class CreditCardFieldsMissingException : ApplicationException
    {
        public CreditCardFieldsMissingException(string message) : base(message)
        {
        }
        public CreditCardFieldsMissingException() : base("Fields are missing")
        {
        }
    }

    // card owner field does not have other credit card information
    public class CreditCardInvalidOwnerNameException : ApplicationException
    {
        public CreditCardInvalidOwnerNameException(string message) : base(message)
        {
        }
        public CreditCardInvalidOwnerNameException() : base("Card owner name contains credit card information")
        {
        }
    }

    // credit card is not expired
    public class CreditCardExpiredException : ApplicationException
    {
        public CreditCardExpiredException(string message) : base(message)
        {
        }
        public CreditCardExpiredException() : base("Is expired")
        {
        }
    }

    // number is valid for specified credit card type
    public class CreditCardNumberInvalidException : ApplicationException
    {
        public CreditCardNumberInvalidException(string message) : base(message)
        {
        }
        public CreditCardNumberInvalidException() : base("Number is invalid for specified credit card type")
        {
        }
    }

    // CVC is valid for specified credit card type
    public class CreditCardCVCInvalidException : ApplicationException
    {
        public CreditCardCVCInvalidException(string message) : base(message)
        {
        }
        public CreditCardCVCInvalidException() : base("CVV is invalid for specified credit card type")
        {
        }
    }
}
