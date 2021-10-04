using Arvato_API_Task.Models.Entities;
using System;

namespace Arvato_API_Task.Models
{
    public interface ICreditCardValidation
    {
        bool ValidateName(string name);
        bool ValidateExpirationDate(DateTime expDate);
        bool ValidateExpirationDate(DateTime expDate, DateTime nowDate);
        (bool result, CCSystem system) ValidateNumber(long value);
        bool ValidateCVV(string value, CCSystem cardType);
    }
}
