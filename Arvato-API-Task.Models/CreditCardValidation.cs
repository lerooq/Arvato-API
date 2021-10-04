using Arvato_API_Task.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Arvato_API_Task.Models
{
    // https://www.freebinchecker.com/credit-card-validator/
    // https://medium.com/hootsuite-engineering/a-comprehensive-guide-to-validating-and-formatting-credit-cards-b9fa63ec7863#c33a

    public class CreditCardValidation : ICreditCardValidation
    {
        private Dictionary<CCSystem, int> lengthsCVV = new Dictionary<CCSystem, int>
        {
            { CCSystem.VISA, 3 },
            { CCSystem.MASTER_CARD, 3 },
            { CCSystem.AMERICAN_EXPRESS, 4 }
        };

        private Dictionary<CCSystem, int> lengthsCCNumber = new Dictionary<CCSystem, int>
        {
            { CCSystem.VISA, 16 },
            { CCSystem.MASTER_CARD, 16 },
            { CCSystem.AMERICAN_EXPRESS, 15 }
        };

        public CreditCardValidation() { }


        private static CCSystem GetCCSystemByIIN(long IIN)
        {
            switch (IIN)
            {
                case 3:
                    return CCSystem.AMERICAN_EXPRESS;
                case 2:
                case 5:
                    return CCSystem.MASTER_CARD;
                case 4:
                    return CCSystem.VISA;
                default:
                    return CCSystem.UNKNOWN;
            }
        }

        // 1: Major Indsutry Identifier (MII)
        // 1-6: Issuer Identification Number (IIN)
        // 7-15: Account Number
        // 16: checksum
        public (bool result, CCSystem system) ValidateNumber(long ccNumber)
        {
            if (ccNumber <= 0)
                return (false, CCSystem.UNKNOWN);

            // Card length check
            int digitCount = MathUtils.GetDigitCount(ccNumber);

            // No credit card types with <15 or >16 digits
            if (digitCount < 15 || digitCount > 16)
                return (false, CCSystem.UNKNOWN);

            // Luhn
            if (!MathUtils.LuhnCheck(ccNumber.ToString()))
                return (false, CCSystem.UNKNOWN);

            // Major Industry Identifier Check
            // First digit of card
            long industryDigit = MathUtils.NthDigitLong(ccNumber, digitCount - 1);
            CCSystem cardSystemType = GetCCSystemByIIN(industryDigit);
            if (cardSystemType == CCSystem.UNKNOWN)
                return (false, CCSystem.UNKNOWN);

            // American express should have a digit count of 15
            // Visa/MasterCard should have digit count of 16
            switch (cardSystemType)
            {
                case CCSystem.UNKNOWN:
                    return (false, CCSystem.UNKNOWN);
                case CCSystem.VISA:
                case CCSystem.MASTER_CARD:
                    if (digitCount < 16)
                        return (false, CCSystem.UNKNOWN);
                    break;
                case CCSystem.AMERICAN_EXPRESS:
                    if (digitCount > 15)
                        return (false, CCSystem.UNKNOWN);
                    break;
            }

            return (true, cardSystemType);
        }

        public bool ValidateCVV(string cvvValue, CCSystem cardType)
        {
            if (string.IsNullOrEmpty(cvvValue))
                return false;

            Regex reg = new Regex(@"^[0-9]+$");
            if (!reg.IsMatch(cvvValue))
                return false;

            int cvvDigitCount = cvvValue.Length;
            int expectedDigitCount = lengthsCVV[cardType];

            if (cvvDigitCount < expectedDigitCount || cvvDigitCount > expectedDigitCount)
                return false;

            return true;
        }

        public bool ValidateExpirationDate(DateTime expDate)
        {
            return ValidateExpirationDate(expDate, DateTime.Now);
        }

        public bool ValidateExpirationDate(DateTime expDate, DateTime nowDate)
        {
            if (expDate == null || nowDate == null)
                throw new ArgumentNullException();

            return nowDate < expDate;
        }

        public bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            Regex reg = new Regex(@"^[a-zA-Z \-æøåäöü']+$");

            return reg.IsMatch(name);
        }
    }
}
