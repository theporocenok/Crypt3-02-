using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace System
{
    //Тесты на простоту числа
    class TestsForSimplicity
    {
        //Тест Поклингтона
        public static bool Poklington(BigInteger n, int t, List<BigInteger> Simples)
        {
            BigInteger[] As = new BigInteger[] { };
            bool check = true;

            for (int i = 0; i < t; i++)
            {
                BigInteger a = BigIntegerRandom.GenerateRandom(1, n, new Random());
                if (BigInteger.ModPow(a, n - 1, n) != 1)
                    return false;

                Array.Resize<BigInteger>(ref As, As.Length + 1);
                As[As.Length - 1] = a;
            }
            for (int i = 0; i < t; i++)
            {
                for (int j = 0; j < Simples.Count; j++)
                {
                    if (BigInteger.ModPow(As[i], (n - 1) / Simples[j], n) == 1)
                    {
                        check = false;
                        break;
                    }

                }
            }
            return check;
        }
        //Вероятностный тест Соловея-Штрассена
        public static bool Solovei_Shtrassen(BigInteger n)
        {
            int count = 0;
            int t = 4;
            Random rnd = new Random((int)(DateTime.Now.Ticks));
            for (int i = 0; i < t; i++)
            {

                BigInteger a = BigIntegerRandom.GenerateRandom(2, n - 2, rnd);

                if (BigInteger.GreatestCommonDivisor(a, n) != 1)
                {
                    break;
                }

                BigInteger Jc = Jakobi(a, n);
                BigInteger S = BigInteger.ModPow(a, (n - 1) / 2, n);

                S = (S + 1 == n) ? S - n : S;

                if (Jc != S)
                {
                    break;
                }

                else
                {
                    count++;
                }
            }
            if (count == t)
            {
                return true;
            }
            return false;
        }
        //Вспомогательный метод для Соловея-Штрассена
        private static int Jakobi(BigInteger a, BigInteger b)
        {

            if (BigInteger.GreatestCommonDivisor(a, b) != 1)
                return 0;

            int r = 1;
            BigInteger c;

            if (a < 0)
            {
                a = -a;
                if (b % 4 == 3)
                    r = -r;
            }

            while (a != 0)
            {
                int i = 0;
                while (a % 2 == 0)
                {
                    i++;
                    a /= 2;
                }
                if (i % 2 != 0)
                    if (b % 8 == 3 || b % 8 == 5)
                        r = -r;

                if (a % 4 == b % 4 && a % 4 == 3)
                    r = -r;
                c = a;
                a = b % c;
                b = c;


            }
            return r;
        }
    }
}
