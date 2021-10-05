using Arvato_API_Task.Models.Entities;
using Arvato_API_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arvato_API_Task
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
                    return string.Join('\n', _errors.Select(e => ErrorStrings[e]));

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

        private static Dictionary<EValidationErrors, string> ErrorStrings = new Dictionary<EValidationErrors, string>() {
            {EValidationErrors.MissingCCInfoBody, "Credit card info missing from body of request" },
            {EValidationErrors.MissingCVV, "CVV is missing"},
            {EValidationErrors.MissingExpirationDate, "Expiration date is invalid or missing" },
            {EValidationErrors.MissingCCNumber, "Number is missing or invalid" },
            {EValidationErrors.MissingOwnerName,  "Owner name is missing"},
            {EValidationErrors.CardExpired,  "Card is expired"},
            {EValidationErrors.OwnerNameInvalid,  "Owner name is invalid"},
            {EValidationErrors.CardNumberInvalid,  "Card number is invalid"},
            {EValidationErrors.CVVInvalid,  "CVV is invalid or doesn't fit card type"}
        };
    }


}
