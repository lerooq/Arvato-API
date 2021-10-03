using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arvato_API_Task.Controllers;
using Moq;
using Arvato_API_Task.Models;
using Arvato_API_Task.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Arvato_API_Task.Tests
{
    [TestClass]
    public class CreditCardControllerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 1234_4567
            };

            var validation = new Mock<ICreditCardValidation>();
            validation.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(true);
            validation.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validation.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validation.Setup(re => re.ValidateNumber(cc.Number)).Returns((true, CCSystem.VISA));

            var controller = new CreditCardController(validation.Object);

            var res = controller.Post(cc);

            Assert.IsInstanceOfType(res, typeof(OkObjectResult));
        }


        
        public void TestMethod1ff()
        {
            /*var repo = new Mock<WeatherForecastController>();
            repo.Setup(re => re.Get()).Returns(new List<WeatherForecast>());*/

            long card = 9321_5678_9012_1239;

            var numDigits = Math.Floor(BigInteger.Log10(card) + 1);

            long getfirstdigit = card;
            while (getfirstdigit > 10)
            {
                getfirstdigit /= 10;
            }

            Console.WriteLine($"{card}\nCard Digits: {numDigits}\nCard System Number: {getfirstdigit}");


            Console.WriteLine("test");

            Assert.IsTrue(true);
        }
    }
}
