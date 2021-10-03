using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arvato_API_Task.Controllers;
using Moq;
using System.Collections.Generic;

namespace Arvato_API_Task.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
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
