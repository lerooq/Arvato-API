using Arvato_API_Task.Models.Entities;
using Arvato_API_Task.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Arvato_API_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardValidation _ccValidator;

        public CreditCardController(ICreditCardValidation ccValidator)
        {
            _ccValidator = ccValidator;
        }

        // GET: api/<CreditCardController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "hello", "world" };
        }

        private (bool result, string[] errors) IsCCInfoNullOrEmpty(CreditCard ccInfo)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(ccInfo.CVV))
                errors.Add("CVV is missing");

            if (ccInfo.ExpirationDate == DateTime.MinValue)
                errors.Add("Expiration date is invalid or missing");

            if (ccInfo.Number == 0 || ccInfo.Number < 0)
                errors.Add("Number is missing or invalid");

            if (string.IsNullOrEmpty(ccInfo.Owner))
                errors.Add("Owner name is missing");

            return (errors.Count == 0, errors.ToArray());
        }

        // POST api/<CreditCardController>
        [HttpPost]
        public IActionResult Post([FromBody] CreditCard creditCardInfo)
        {
            if (creditCardInfo == null)
                return BadRequest("Credit card info missing from body of request");

            // Check if cc info is missing data
            var (nullCheckResult, nullCheckErrors) = IsCCInfoNullOrEmpty(creditCardInfo);
            if (!nullCheckResult)
                return BadRequest(nullCheckErrors.ConcatWithDelimeter('\n'));

            List<string> errors = new List<string>();

            if(!_ccValidator.ValidateExpirationDate(creditCardInfo.ExpirationDate))
                errors.Add("Card is expired");

            if (!_ccValidator.ValidateName(creditCardInfo.Owner))
                errors.Add("Owner name is invalid");

            var (isCardNumberValid, cardSystem) = _ccValidator.ValidateNumber(creditCardInfo.Number);

            if (!isCardNumberValid)
                errors.Add("Card number is invalid");

            if (isCardNumberValid)
                if (!_ccValidator.ValidateCVV(creditCardInfo.CVV, cardSystem))
                    errors.Add("CVV is invalid or doesn't fit card type");

            if (errors.Count > 0)
                return BadRequest(errors.ConcatWithDelimeter('\n'));

            switch (cardSystem)
            {
                case CCSystem.AMERICAN_EXPRESS:
                    return Ok("American Express");
                case CCSystem.MASTER_CARD:
                    return Ok("Master Card");
                case CCSystem.VISA:
                    return Ok("Visa");
                default:
                    return BadRequest("Unknown or unsupported card type");
            }
        }



    }
}
