using Arvato_API_Task.Models.Entities;
using Arvato_API_Task.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Arvato_API_Task.Models
{
    // https://www.freebinchecker.com/credit-card-validator/
    // https://medium.com/hootsuite-engineering/a-comprehensive-guide-to-validating-and-formatting-credit-cards-b9fa63ec7863#c33a

    public class CreditCardValidationHelper : ICreditCardValidationHelper
    {
        public static readonly Regex REGEX_OWNER_NAME = new Regex(@"^[a-zA-Z \-æøåäöü']+$");
        public static readonly Regex REGEX_CVV = new Regex(@"^[0-9]+$");

        public CreditCardValidationHelper() { }


        private static int GetCVVLengthByCCSystem(CCSystem system)
        {
            switch (system)
            {
                case CCSystem.VISA:
                case CCSystem.MASTER_CARD:
                    return 3;
                case CCSystem.AMERICAN_EXPRESS:
                    return 4;
                default:
                    return -1;
            }
        }

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
        public CCSystem ValidateNumber(long ccNumber)
        {
            if (ccNumber <= 0)
                return CCSystem.UNKNOWN;

            // Card length check
            int digitCount = MathUtils.GetDigitCount(ccNumber);

            // No credit card types with <15 or >16 digits
            if (digitCount < 15 || digitCount > 16)
                return CCSystem.UNKNOWN;

            // Luhn
            if (!MathUtils.LuhnCheck(ccNumber.ToString()))
                return CCSystem.UNKNOWN;

            // Major Industry Identifier Check
            // First digit of card
            long industryDigit = MathUtils.NthDigitLong(ccNumber, digitCount - 1);
            CCSystem cardSystemType = GetCCSystemByIIN(industryDigit);
            if (cardSystemType == CCSystem.UNKNOWN)
                return CCSystem.UNKNOWN;

            // American express should have a digit count of 15
            // Visa/MasterCard should have digit count of 16
            switch (cardSystemType)
            {
                case CCSystem.UNKNOWN:
                    return CCSystem.UNKNOWN;
                case CCSystem.VISA:
                case CCSystem.MASTER_CARD:
                    if (digitCount < 16)
                        return CCSystem.UNKNOWN;
                    break;
                case CCSystem.AMERICAN_EXPRESS:
                    if (digitCount > 15)
                        return CCSystem.UNKNOWN;
                    break;
            }

            return cardSystemType;
        }

        public bool ValidateCVV(string cvvValue, CCSystem cardType)
        {
            if (string.IsNullOrEmpty(cvvValue))
                return false;

            // CVV only contains numbers
            
            if (!REGEX_CVV.IsMatch(cvvValue))
                return false;

            int cvvDigitCount = cvvValue.Length;
            int expectedCvvDigitCount = GetCVVLengthByCCSystem(cardType);

            if (cvvDigitCount != expectedCvvDigitCount)
                return false;

            return true;
        }

        public bool ValidateExpirationDate(DateTime expDate)
        {
            return ValidateExpirationDate(expDate, DateTime.Now);
        }

        public bool ValidateExpirationDate(DateTime expDate, DateTime nowDate)
        {
            return nowDate < expDate;
        }

        public bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            return REGEX_OWNER_NAME.IsMatch(name);
        }
    }
}
