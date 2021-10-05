namespace Arvato_API_Task.Models
{
    public enum EValidationErrors
    {
        MissingCCInfoBody,
        MissingCVV,
        MissingExpirationDate,
        MissingCCNumber,
        MissingOwnerName,
        CardExpired,
        OwnerNameInvalid,
        CardNumberInvalid,
        CVVInvalid
    }
}
