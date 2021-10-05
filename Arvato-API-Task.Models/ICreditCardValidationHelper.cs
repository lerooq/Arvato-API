using Arvato_API_Task.Models.Entities;
using System;

namespace Arvato_API_Task.Models
{
    public interface ICreditCardValidationHelper
    {
        bool ValidateName(string name);
        bool ValidateExpirationDate(DateTime expDate);
        bool ValidateExpirationDate(DateTime expDate, DateTime nowDate);
        CCSystem ValidateNumber(long value);
        bool ValidateCVV(string value, CCSystem cardType);
    }
}
