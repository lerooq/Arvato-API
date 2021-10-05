using Arvato_API_Task;
using Arvato_API_Task.Models;
using Arvato_API_Task.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arvato_API_Task.Tests
{
    [TestClass]
    public class CreditCardValidatorTests
    {
        #region Valid_Tests

        [TestMethod]
        public void CreditCardValidator_Valid_Visa_NoErrors()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil Rysgaard",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 4539_0246_2079_1394
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            validationHelper.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(true);
            validationHelper.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validationHelper.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validationHelper.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.VISA);

            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsFalse(validator.HasErrors);
            Assert.IsTrue(validator.Result == CCSystem.VISA);
        }

        [TestMethod]
        public void CreditCardValidator_Valid_MasterCard_NoErrors()
        {
            var cc = new CreditCard()
            {
                CVV = "0271",
                Owner = "Emil Rysgaard",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 5548455237884311
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            validationHelper.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.MASTER_CARD)).Returns(true);
            validationHelper.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validationHelper.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validationHelper.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.MASTER_CARD);

            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsFalse(validator.HasErrors);
            Assert.IsTrue(validator.Result == CCSystem.MASTER_CARD);
        }

        [TestMethod]
        public void CreditCardValidator_Valid_AmericanExpress_NoErrors()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil Rysgaard",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 379294575750114
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            validationHelper.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.MASTER_CARD)).Returns(true);
            validationHelper.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validationHelper.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validationHelper.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.MASTER_CARD);

            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsFalse(validator.HasErrors);
            Assert.IsTrue(validator.Result == CCSystem.MASTER_CARD);
        }

        #endregion

        #region NullEmpty_Tests

        [TestMethod]
        public void CreditCardValidator_NullCC_HasErrors()
        {
            var validationHelper = new Mock<ICreditCardValidationHelper>();
            var validator = new CreditCardValidator(null, validationHelper.Object);

            Assert.IsTrue(validator.HasErrors);
            Assert.IsTrue(validator.ValidationErrors.Count == 1);
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.MissingCCInfoBody));
        }

        [TestMethod]
        public void CreditCardValidator_NullEmpty_HasErrors()
        {
            var cc = new CreditCard()
            {
                CVV = null,
                Owner = "",
                ExpirationDate = DateTime.MinValue,
                Number = -1
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsTrue(validator.HasErrors);
            Assert.IsTrue(validator.ValidationErrors.Count == 4);
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.MissingCCNumber));
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.MissingOwnerName));
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.MissingExpirationDate));
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.MissingCVV));
        }

        #endregion

        #region WrongFields_Tests

        [TestMethod]
        public void CreditCardValidator_CardExpired_HasErrors()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil Rysgaard",
                ExpirationDate = new DateTime(2005, 3, 4),
                Number = 4539_0246_2079_1394
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            validationHelper.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(true);
            validationHelper.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(false);
            validationHelper.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validationHelper.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.VISA);

            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsTrue(validator.HasErrors);
            Assert.IsTrue(validator.ValidationErrors.Count == 1);
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.CardExpired));
        }

        [TestMethod]
        public void CreditCardValidator_WrongCardNumber_HasErrors()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil Rysgaard",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 4539_0246_2079_1321
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            validationHelper.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(true);
            validationHelper.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validationHelper.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validationHelper.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.UNKNOWN);

            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsTrue(validator.HasErrors);
            Assert.IsTrue(validator.ValidationErrors.Count == 1);
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.CardNumberInvalid));
        }

        [TestMethod]
        public void CreditCardValidator_OwnerNameCorrupt_HasErrors()
        {
            var cc = new CreditCard()
            {
                CVV = "027",
                Owner = "Emil Rysgaard954",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 4539_0246_2079_1394
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            validationHelper.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(true);
            validationHelper.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validationHelper.Setup(re => re.ValidateName(cc.Owner)).Returns(false);
            validationHelper.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.VISA);

            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsTrue(validator.HasErrors);
            Assert.IsTrue(validator.ValidationErrors.Count == 1);
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.OwnerNameInvalid));
        }

        [TestMethod]
        public void CreditCardValidator_WrongCVVForType_HasErrors()
        {
            var cc = new CreditCard()
            {
                CVV = "0275",
                Owner = "Emil Rysgaard",
                ExpirationDate = new DateTime(2030, 3, 4),
                Number = 4539_0246_2079_1394
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            validationHelper.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(false);
            validationHelper.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(true);
            validationHelper.Setup(re => re.ValidateName(cc.Owner)).Returns(true);
            validationHelper.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.VISA);

            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsTrue(validator.HasErrors);
            Assert.IsTrue(validator.ValidationErrors.Count == 1);
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.CVVInvalid));
        }

        [TestMethod]
        public void CreditCardValidator_AllCorruptFields_HasErrors()
        {
            var cc = new CreditCard()
            {
                CVV = "02751",
                Owner = "Emil Rysgaard038",
                ExpirationDate = new DateTime(2005, 3, 4),
                Number = 4539_0246_2079_1392
            };

            var validationHelper = new Mock<ICreditCardValidationHelper>();
            validationHelper.Setup(re => re.ValidateCVV(cc.CVV, CCSystem.VISA)).Returns(false);
            validationHelper.Setup(re => re.ValidateExpirationDate(cc.ExpirationDate)).Returns(false);
            validationHelper.Setup(re => re.ValidateName(cc.Owner)).Returns(false);
            validationHelper.Setup(re => re.ValidateNumber(cc.Number)).Returns(CCSystem.UNKNOWN);

            var validator = new CreditCardValidator(cc, validationHelper.Object);

            Assert.IsTrue(validator.HasErrors);
            Assert.IsTrue(validator.ValidationErrors.Count == 3);

            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.CardNumberInvalid));
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.OwnerNameInvalid));
            Assert.IsTrue(validator.ValidationErrors.Contains(EValidationErrors.CardExpired));
        }

        #endregion
    }
}
