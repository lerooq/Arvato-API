using System;
using System.Linq;
using Arvato_API_Task.Models;
using Arvato_API_Task.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arvato_API_Task.Tests
{
    [TestClass]
    public class CreditCardValidationTests
    {
        #region ValidateNumber_Tests

        [TestMethod]
        public void ValidateNumber_Bogus_Fails()
        {
            var ccList = new long[]
            {
                1111111111111111, 9999999999999999, -4485089979925707,
                -4485089979925703, 4532493273012637, 000000010
            };

            CreditCardValidationHelper ccv = new CreditCardValidationHelper();

            var results = ccList.Select(cc => ccv.ValidateNumber(cc));

            foreach (var result in results)
            {
                Assert.IsTrue(result == CCSystem.UNKNOWN);
            }
        }

        [TestMethod]
        public void ValidateNumber_VISA_Passes()
        {
            var ccList = new long[]
            {
                4929098150149056, 4716184391778015, 4485089979925707,
                4929240306217870, 4532493273012639, 4929647639220019
            };

            CreditCardValidationHelper ccv = new CreditCardValidationHelper();

            var results = ccList.Select(cc => ccv.ValidateNumber(cc));

            foreach (var result in results)
            {
                Assert.IsTrue(result == CCSystem.VISA);
            }
        }

        [TestMethod]
        public void ValidateNumber_MasterCard_Passes()
        {
            var ccList = new long[]
            {
                5453967042791609, 5422632533114533, 5366358843954420,
                5111274718022904, 5498312771524771, 5549177062364446
            };

            CreditCardValidationHelper ccv = new CreditCardValidationHelper();

            var results = ccList.Select(cc => ccv.ValidateNumber(cc));

            foreach (var result in results)
            {
                Assert.IsTrue(result == CCSystem.MASTER_CARD);
            }
        }

        [TestMethod]
        public void ValidateNumber_AmericanExpress_Passes()
        {
            var ccList = new long[]
            {
                341912562027643, 342906101811565, 371379136889747,
                345617871770199, 379593948617680, 377778089862144
            };

            CreditCardValidationHelper ccv = new CreditCardValidationHelper();

            var results = ccList.Select(cc => ccv.ValidateNumber(cc));

            foreach (var result in results)
            {
                Assert.IsTrue(result == CCSystem.AMERICAN_EXPRESS);
            }
        }


        [TestMethod]
        public void ValidateNumber_AmericanExpress_TooLongTooShort_Fails()
        {
            var ccList = new long[]
            {
                34191256202764333, 34290665
            };

            CreditCardValidationHelper ccv = new CreditCardValidationHelper();

            var results = ccList.Select(cc => ccv.ValidateNumber(cc));

            foreach (var result in results)
            {
                Assert.IsTrue(result == CCSystem.UNKNOWN);
            }
        }


        [TestMethod]
        public void ValidateNumber_ShortNumber_Fails()
        {
            long ccNumber = 4372_4948_1923;
            CreditCardValidationHelper ccv = new CreditCardValidationHelper();

            bool result = ccv.ValidateNumber(ccNumber) == CCSystem.UNKNOWN;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateNumber_BogusNumbers_Fails()
        {
            var ccNumbers = new long[] { 0, -1 };
            CreditCardValidationHelper ccv = new CreditCardValidationHelper();

            var results = ccNumbers.Select(num => ccv.ValidateNumber(num) == CCSystem.UNKNOWN);

            foreach (var result in results)
                Assert.IsTrue(result);
        }

        #endregion

        #region ValidateCVV_Tests

        [TestMethod]
        public void ValidateCVV_BadInput_Fails()
        {
            var cvvs = new string[] { "!#04", "1ø3" };
            var ccv = new CreditCardValidationHelper();

            var results = cvvs.Select(cvv => ccv.ValidateCVV(cvv, CCSystem.AMERICAN_EXPRESS));

            foreach (var result in results)
                Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateCVV_NullEmpty_Fails()
        {
            var ccv = new CreditCardValidationHelper();

            var result1 = ccv.ValidateCVV(null, CCSystem.AMERICAN_EXPRESS);
            var result2 = ccv.ValidateCVV("", CCSystem.AMERICAN_EXPRESS);

            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void ValidateCVV_AmericanExpress_ValidCVV_Passes()
        {
            var cvv = "0415";
            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateCVV(cvv, CCSystem.AMERICAN_EXPRESS);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateCVV_AmericanExpress_VisaCVV_Fails()
        {
            var cvv = "041";
            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateCVV(cvv, CCSystem.AMERICAN_EXPRESS);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateCVV_VISA_MC_ValidCVV_Passes()
        {
            var cvv = "041";
            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateCVV(cvv, CCSystem.VISA);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateCVV_AMERICAN_EXPRESES_TooManyDigits_Fail()
        {
            var cvv = "544555";
            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateCVV(cvv, CCSystem.AMERICAN_EXPRESS);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateCVV_VISA_MC_TooManyDigits_Fail()
        {
            var cvv = "5445";
            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateCVV(cvv, CCSystem.VISA);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateCVV_TooFewDigits_Fail()
        {
            var cvv = "54";
            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateCVV(cvv, CCSystem.VISA);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateCVV_NegativeNum_Fails()
        {
            var cvv = "-1";
            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateCVV(cvv, CCSystem.VISA);

            Assert.IsFalse(result);
        }

        #endregion

        #region ValidateExpirationDate_Tests


        [TestMethod]
        public void ValidateExpirationDate_Expired_Fails()
        {
            var cardDate = new DateTime(2017, 01, 21);
            var presentDate = new DateTime(2018, 01, 21);

            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateExpirationDate(cardDate, presentDate);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateExpirationDate_Valid_Passes()
        {
            var cardDate = new DateTime(2020, 01, 21);
            var presentDate = new DateTime(2018, 01, 21);

            var ccv = new CreditCardValidationHelper();

            var result = ccv.ValidateExpirationDate(cardDate, presentDate);
            Assert.IsTrue(result);
        }
        #endregion

        #region ValidateName_Tests

        [TestMethod]
        public void ValidateName_Name_Passes()
        {
            string[] names = { "Emil Rysgaard", "Emil Ernst Rysgaard" };

            var ccv = new CreditCardValidationHelper();

            var results = names.Select(name => ccv.ValidateName(name));

            foreach (var result in results)
                Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateName_DodgyName_Passes()
        {
            string name = "L'Carl-Emil Ernst Düsseldorph";

            var ccv = new CreditCardValidationHelper();

            bool res = ccv.ValidateName(name);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void ValidateName_Null_Fails()
        {
            CreditCardValidationHelper ccv = new CreditCardValidationHelper();
            Assert.IsFalse(ccv.ValidateName(null));
        }

        [TestMethod]
        public void ValidateName_EmptyString_Fails()
        {
            var ccv = new CreditCardValidationHelper();
            Assert.IsFalse(ccv.ValidateName(""));
        }

        [TestMethod]
        public void ValidateName_Name_CCNumber_Fails()
        {
            string[] names = { "4573203984732918", "4573203984732918Emil Rysgaard 043", "4573203984732918Emil Rysgaard 043" };

            var ccv = new CreditCardValidationHelper();

            var results = names.Select(name => ccv.ValidateName(name));

            foreach (var result in results)
                Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateName_Name_CVC_Fails()
        {
            string[] names = { "Emil Rysgaard 027", "027 Emil Rysgaard " };

            var ccv = new CreditCardValidationHelper();

            var results = names.Select(name => ccv.ValidateName(name));

            foreach (var result in results)
                Assert.IsFalse(result);
        }

        #endregion
    }
}
