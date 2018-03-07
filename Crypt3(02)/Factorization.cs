using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace System
{
    //Разложение больших чисел на простые множители
    public static class Factorization
    {
        private static Random r = new Random();

        public static BigInteger run(BigInteger n)
        {
            int simpleResult = simple(n);
            if (simpleResult != 0)
                return simpleResult;

            BigInteger x = r.Next(2, int.MaxValue);
            BigInteger y = 1;
            BigInteger i = 0;
            BigInteger stage = 2;
            BigInteger gcd;
            while ((gcd = BigInteger.GreatestCommonDivisor(n, BigInteger.Abs(x - y))) == 1)
            {
                if (i == stage)
                {
                    y = x;
                    stage = stage * 2;
                }
                x = BigInteger.ModPow(x, 2, n) + 1;
                i = i + 1;
            }
            return gcd;
        }

        private static int simple(BigInteger n)
        {
            if (n % 2 == 0)
                return 2;
            int i = 3;
            while (i < 10000)
            {
                if (n % i == 0)
                    return i;
                i += 2;
            }
            return 0;
        }

        public static BigInteger fullSimple(BigInteger n)
        {
            if (n % 2 == 0)
                return 2;
            BigInteger i = 3;
            BigInteger sqrt = n.Sqrt();
            while (i < sqrt)
            {
                if (n % i == 0)
                    return i;
                i += 2;
            }
            return n;
        }

        private static BigInteger Sqrt(this BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!isSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static Boolean isSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }
    }
}