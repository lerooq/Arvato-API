using Arvato_API_Task.Models.Entities;
using Arvato_API_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arvato_API_Task.Models
{
    public class CreditCardValidator
    {
        public CCSystem Result;
        public List<EValidationErrors> ValidationErrors => _errors;
        public bool HasErrors => _errors.Count > 0;
        private List<EValidationErrors> _errors = new List<EValidationErrors>();

        public CreditCardValidator(CreditCard creditCardInfo, ICreditCardValidationHelper ccValidator)
        {
            Result = CCSystem.UNKNOWN;

            if (creditCardInfo == null)
            {
                _errors.Add(EValidationErrors.MissingCCInfoBody);
                return;
            }

            if (string.IsNullOrEmpty(creditCardInfo.CVV))
                _errors.Add(EValidationErrors.MissingCVV);

            if (creditCardInfo.ExpirationDate == DateTime.MinValue)
                _errors.Add(EValidationErrors.MissingExpirationDate);

            if (creditCardInfo.Number <= 0)
                _errors.Add(EValidationErrors.MissingCCNumber);

            if (string.IsNullOrEmpty(creditCardInfo.Owner))
                _errors.Add(EValidationErrors.MissingOwnerName);

            if (HasErrors)
                return;

            if (!ccValidator.ValidateExpirationDate(creditCardInfo.ExpirationDate))
                _errors.Add(EValidationErrors.CardExpired);

            if (!ccValidator.ValidateName(creditCardInfo.Owner))
                _errors.Add(EValidationErrors.OwnerNameInvalid);

            CCSystem cardSystem = ccValidator.ValidateNumber(creditCardInfo.Number);

            if (cardSystem == CCSystem.UNKNOWN)
                _errors.Add(EValidationErrors.CardNumberInvalid);
            else
            {
                if (!ccValidator.ValidateCVV(creditCardInfo.CVV, cardSystem))
                    _errors.Add(EValidationErrors.CVVInvalid);
            }

            if (HasErrors)
                return;

            Result = cardSystem;
        }

        public string ResultAsString
        {
            get
            {
                if (HasErrors)
                    return string.Join('\n', _errors.Select(e => GetErrorStringByEnum(e)));

                switch (Result)
                {
                    case CCSystem.AMERICAN_EXPRESS:
                        return "American Express";
                    case CCSystem.MASTER_CARD:
                        return "Master Card";
                    case CCSystem.VISA:
                        return "Visa";
                    default:
                        return "Unknown";
                }
            }
        }

        public string GetErrorStringByEnum(EValidationErrors valErrorEnum)
        {
            switch (valErrorEnum)
            {
                case EValidationErrors.MissingCCInfoBody:
                    return "Credit card info missing from body of request";
                case EValidationErrors.MissingCVV:
                    return "CVV is missing";
                case EValidationErrors.MissingExpirationDate:
                    return "Expiration date is invalid or missing";
                case EValidationErrors.MissingCCNumber:
                    return "Number is missing or invalid";
                case EValidationErrors.MissingOwnerName:
                    return "Owner name is missing";
                case EValidationErrors.CardExpired:
                    return "Card is expired";
                case EValidationErrors.OwnerNameInvalid:
                    return "Owner name is invalid";
                case EValidationErrors.CardNumberInvalid:
                    return "Card number is invalid";
                case EValidationErrors.CVVInvalid:
                    return "CVV is invalid or doesn't fit card type";
                default:
                    return "Unknown error";
            }
        }
    }


}
