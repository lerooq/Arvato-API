using Arvato_API_Task.Models.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Arvato_API_Task.Tests
{
    [TestClass]
    public class MathUtilTests
    {
        [TestMethod]
        public void GetDigitCountLong_NegativeNumPositiveResult()
        {
            long testValue = -1234;

            int digitCount = MathUtils.GetDigitCount(testValue);

            Assert.AreEqual(digitCount, 4);
            Assert.IsTrue(digitCount > 0);
        }

        [TestMethod]
        public void GetDigitCountLong_NegativeNumTrueResult()
        {
            long testValue = -1234;

            int digitCount = MathUtils.GetDigitCount(testValue);

            Assert.AreEqual(digitCount, 4);
        }

        [TestMethod]
        public void GetDigitCountLong_CreditCard16Digits()
        {
            long testValue = 4572_1234_5678_8765;

            int digitCount = MathUtils.GetDigitCount(testValue);

            Assert.AreEqual(digitCount, 16);
        }

        [TestMethod]
        public void NthDigitLong_NegativeDigitThrowsException()
        {
            long testValue = 4572_1234_5678_8765;
            Assert.ThrowsException<ArgumentException>(() => MathUtils.NthDigitLong(testValue, -2));
        }

        [TestMethod]
        public void NthDigitLong_NegativeNumber_Returns_Abs()
        {
            long testValue = -4572_1234_5678_8765;

            long result = MathUtils.NthDigitLong(testValue, 15);

            Assert.IsTrue(result == 4);
        }

        [TestMethod]
        public void NthDigitLong_CC_14thDigit_Passes()
        {
            long testValue = 4572_1234_5678_8765;

            long result = MathUtils.NthDigitLong(testValue, 14);

            Assert.IsTrue(result == 5);
        }

        [TestMethod]
        public void Luhn_CreditCard_AmericanExpress_WrongChecksum_Fails()
        {
            long testValue = 3758_4122_0838_828;

            bool res = MathUtils.LuhnCheck(testValue.ToString());

            Assert.IsFalse(res);
        }

        [TestMethod]
        public void Luhn_CreditCard_AmericanExpress_Passes()
        {
            long testValue = 3758_4122_0838_829;

            bool res = MathUtils.LuhnCheck(testValue.ToString());

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Luhn_CreditCard_Visa_Passes()
        {
            long testValue = 4556_9229_0172_8621;

            bool res = MathUtils.LuhnCheck(testValue.ToString());

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Luhn_BogusCard_Fails()
        {
            long testValue = 9999_9999_1111_1111;

            bool res = MathUtils.LuhnCheck(testValue.ToString());

            Assert.IsFalse(res);
        }
    }
}
