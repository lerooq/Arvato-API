using System;
using System.Linq;
using System.Numerics;

namespace Arvato_API_Task.Models
{
    public static class MathUtils
    {
        public static int GetDigitCount(int value)
        {
            return (int)Math.Floor(Math.Log10(Math.Abs(value)) + 1);
        }

        public static int GetDigitCount(long value)
        {
            return (int)Math.Floor(BigInteger.Log10(BigInteger.Abs(value)) + 1);
        }

        public static long NthDigitLong(long value, int digitPlace)
        {
            if (digitPlace < 0) throw new ArgumentException();
            if (value < 0)
                value = Math.Abs(value);

            while (digitPlace-- > 0)
                value /= 10;

            long digit = value % 10;
            return digit;
        }


        /// <summary>
        /// Luhn Algorithm C#
        /// This class is created on 13 March 2019, at 17:17 (UTC).
        /// by Lakmal http://github.com/l4km47
        /// </summary>
        /// <param name="ccNumber">Credit/Debit/etc card number</param>
        /// <returns>bool</returns>
        public static bool LuhnCheck(string ccNumber)
        {
            int sum = 0;
            bool alternate = false;
            for (int i = ccNumber.Length - 1; i >= 0; i--)
            {
                char[] nx = ccNumber.ToArray();
                int n = int.Parse(nx[i].ToString());

                if (alternate)
                {
                    n *= 2;

                    if (n > 9)
                    {
                        n = (n % 10) + 1;
                    }
                }
                sum += n;
                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }
    }
}
