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
    public class CreditCardApiControllerTests
    {
        [TestMethod]
        public void Post_NullBody_BadRequest_Fails()
        {
            var validation = new Mock<ICreditCardValidationHelper>();
            var controller = new CreditCardController(validation.Object);

            var result = controller.Post(null);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            var message = (result as BadRequestObjectResult).Value as string;
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void Post_Missing_All_Fails_Msg_HasAllLines()
        {
            var cc = new CreditCard() { };

            var validation = new Mock<ICreditCardValidationHelper>();
            var controller = new CreditCardController(validation.Object);

            var result = (BadRequestObjectResult)controller.Post(cc);
            var resultMessage = (string)result.Value;

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.IsTrue(resultMessage.Split('\n').Length == 4);
        }

        [TestMethod]
        public void Post_Invalid_Expiration_CVV_BadRequest_Fails()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil",
                ExpirationDate = new DateTime(205, 3, 4),
                Number = 1234_4567
            };

            var validation = new Mock<ICreditCardValidationHelper>();
            validation.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(false);
            validation.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(false);
            validation.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validation.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.VISA);

            var controller = new CreditCardController(validation.Object);

            var res = controller.Post(cc);
            Assert.IsInstanceOfType(res, typeof(BadRequestObjectResult));

            var resultMessage = (res as BadRequestObjectResult).Value as string;
            Assert.IsTrue(resultMessage.Split('\n').Length == 2);
        }

        [TestMethod]
        public void Post_Valid_Visa_Returns_Ok_With_CardType()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 1234_4567
            };

            var validation = new Mock<ICreditCardValidationHelper>();
            validation.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(true);
            validation.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validation.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validation.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.VISA);

            var controller = new CreditCardController(validation.Object);

            var res = controller.Post(cc);
            Assert.IsInstanceOfType(res, typeof(OkObjectResult));

            string resultMessage = (res as OkObjectResult).Value as string;
            Assert.IsTrue(resultMessage.Equals("Visa"));
        }

        [TestMethod]
        public void Post_Valid_MasterCard_Returns_Ok_With_CardType()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 1234_4567
            };

            var validation = new Mock<ICreditCardValidationHelper>();
            validation.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.MASTER_CARD)).Returns(true);
            validation.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validation.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validation.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.MASTER_CARD);

            var controller = new CreditCardController(validation.Object);

            var res = controller.Post(cc);
            Assert.IsInstanceOfType(res, typeof(OkObjectResult));

            string resultMessage = (res as OkObjectResult).Value as string;
            Assert.IsTrue(resultMessage.Equals("Master Card"));
        }

        [TestMethod]
        public void Post_Valid_AmericanExpress_Returns_Ok_With_CardType()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 1234_4567
            };

            var validation = new Mock<ICreditCardValidationHelper>();
            validation.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.AMERICAN_EXPRESS)).Returns(true);
            validation.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validation.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validation.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.AMERICAN_EXPRESS);

            var controller = new CreditCardController(validation.Object);

            var res = controller.Post(cc);
            Assert.IsInstanceOfType(res, typeof(OkObjectResult));

            string resultMessage = (res as OkObjectResult).Value as string;
            Assert.IsTrue(resultMessage.Equals("American Express"));
        }
    }
}
