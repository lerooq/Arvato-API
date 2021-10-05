using Arvato_API_Task.Models.Entities;
using Arvato_API_Task.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Arvato_API_Task.Controllers
{
    // localhost:xxxxx/api/creditcard/
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardValidationHelper _ccValidator;

        public CreditCardController(ICreditCardValidationHelper ccValidator)
        {
            _ccValidator = ccValidator;
        }

        // POST api/<CreditCardController>
        [HttpPost]
        public IActionResult Post([FromBody] CreditCard creditCardInfo)
        {
            var validator = new CreditCardValidator(creditCardInfo, _ccValidator);

            if (validator.HasErrors)
                return BadRequest(validator.ResultAsString);
            else
                return Ok(validator.ResultAsString);
        }
    }


}
