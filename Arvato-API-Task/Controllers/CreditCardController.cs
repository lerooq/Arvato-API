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

        // POST api/<CreditCardController>
        [HttpPost]
        public IActionResult Post([FromBody] CreditCard creditCardInfo)
        {
            if (creditCardInfo == null)
                return BadRequest("Credit card info missing from body of request");

            bool error = false;
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(creditCardInfo.CVV))
            {
                error = true;
                errors.Add("CVV is missing");
            }
            
            if(creditCardInfo.ExpirationDate == DateTime.MinValue)
            {
                error = true;
                errors.Add("Expiration date is invalid or missing");
            }
            
            //if(creditCardInfo.)


            return Ok("ok");
        }

    }
}
