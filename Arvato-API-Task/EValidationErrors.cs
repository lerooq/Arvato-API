namespace Arvato_API_Task
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
