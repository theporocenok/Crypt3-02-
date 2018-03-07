using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace System
{
    static class GCDEX
    {
        private static BigInteger X, Y, NOD;

        public static BigInteger GetNOD(BigInteger a, BigInteger b)
        {
            gcdex(a, b);
            return NOD;
        }

        public static BigInteger GetX(BigInteger a, BigInteger b)
        {
            gcdex(a, b);
            return X;
        }
        public static BigInteger GetY(BigInteger a, BigInteger b)
        {
            gcdex(a, b);
            return Y;
        }

        public static BigInteger Max(BigInteger a, BigInteger b)
        {
            gcdex(a, b);
            return (X>Y)?X:Y;
        }
        //Расширенный алгоритм Евклида
        private static void gcdex(BigInteger a, BigInteger b)
        {
            BigInteger q, r, x1, x2, y1, y2;
            if (b == 0)
            {
                NOD = a; X = 1; Y = 0;
                return;
            }
            x2 = 1; x1 = 0; y2 = 0; y1 = 1;
            while (b > 0)
            {
                q = a / b; r = a - q * b;
                X = x2 - q * x1; Y = y2 - q * y1;
                a = b; b = r;
                x2 = x1; x1 = X; y2 = y1; y1 = Y;
            }
            NOD = a; X = x2; Y = y2;
        }
    }
}
