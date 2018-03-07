using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace System
{
    class DigitalSignature
    {
        private BigInteger p;//Большое простое число
        private int g;//Порождающий элемент
        private static List<BigInteger> SimplesList = new List<BigInteger>();//Простые множители p
        private BigIntegerRandom BIR;

        public DigitalSignature(int SizeOfSimple)
        {
            BIR = new BigIntegerRandom();
            P = BigSimpleElgamal(SizeOfSimple);
            SimpleMultipliers(P - 1);
            g = FindG();
        }

        public DigitalSignature(int SizeOfSimple, bool NeedInfoForConsole)
        {
            BIR = new BigIntegerRandom();
            Console.WriteLine("Генерация числа p");
            P = BigSimpleElgamal(SizeOfSimple);
            Console.WriteLine("Разложение числа на простые множители");
            SimpleMultipliers(P - 1);
            Console.WriteLine("Нахождение G");
            g = FindG();
        }

        //Свойства

        //Большое простое число
        public BigInteger P
        {
            get { return p; }
            set { p = value; }
        }

        //Порождающий элемент
        public int G
        {
            get { return g; }
            set { g = value; }
        }

        //Основные функции

        //Нахождение G
        private int FindG()
        {
            int g = 1;
            bool check = false;
            int count;
            while (!check)
            {
                count = 0;
                g++;
                foreach (BigInteger q in SimplesList)
                    if (BigInteger.ModPow(g, (P - 1) / q, P) == 1)
                    {
                        count++;
                        break;
                    }
                if (count == 0)
                    check = true;
            }
            return g;
        }

        //Генерация большого простого числа по алгоритму Эль-Гамаля
        private BigInteger BigSimpleElgamal(int NeededSize)
        {
            bool check = false;
            BigInteger n = 0;//Большое простое число
            BigInteger F = 0;
            BigInteger R = 0;
            List<BigInteger> SimplesForN = null;
            while (!check)
            {
                SimplesForN = new List<BigInteger>();
                F = BIR.MakeF(NeededSize / 2 + 1, ref SimplesForN);
                R = BIR.MakeF(NeededSize / 2);
                R = R >> 1;
                R = R << 1;
                n = R * F + 1;
                check = TestsForSimplicity.Poklington(n, 100, SimplesForN);
            }
            SimplesList = SimplesForN;
            return n;
        }

        //Цифровая подпись
        /// <summary>
        /// Цифровая подпись сообщения h по схеме Эль-Гамаля
        /// </summary>
        /// <param name="h">Хэш сообщения</param>
        /// <param name="s"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public bool ElGamal_DigitalSignature(BigInteger h, ref BigInteger s, ref BigInteger r)
        {
            BigInteger k, a, d;
            do
            {
                k = BigIntegerRandom.GenerateRandom(0, P, new Random());//Случайное число
                d = BigInteger.ModPow(G, k, P);//Вычисление открытого ключа
                a = BIR.MakeF(32);
                while (BigInteger.GreatestCommonDivisor(a, P - 1) != 1)
                    a = BIR.MakeF(32);
                s = BigInteger.ModPow(G, a, P);
                BigInteger reverseElement = GCDEX.GetX(a, P - 1);
                r = BigInteger.ModPow(reverseElement * (h - s * k), 1, P - 1);
            } while (r < 0 || !ElGamal_Verification(d, s, r, G, h, P));
            return true;
        }

        //Цифровая подпись по примерам
        public static void ElGamal_DigitalSignature(BigInteger n, BigInteger g, BigInteger k, BigInteger a, BigInteger h, ref BigInteger s, ref BigInteger r)
        {
            BigInteger d = BigInteger.ModPow(g, k, n);//Вычисление открытого ключа
            s = BigInteger.ModPow(g, a, n);
            BigInteger reverseElement = GCDEX.GetX(a, n - 1);
            r = BigInteger.ModPow(reverseElement * (h - s * k), 1, n - 1);
            //Console.WriteLine("p={0}\ng={1}\nk={2}\nd={3}\nh={4}\na={5}\ns={6}\nr={7}", n, g, k, d, h, a, s, r);
        }

        //Проверка ЭЦП
        private bool ElGamal_Verification(BigInteger d, BigInteger s, BigInteger r, BigInteger g, BigInteger h, BigInteger n)
        {
            return (BigInteger.ModPow(BigInteger.ModPow(d, s, n) * BigInteger.ModPow(s, r, n), 1, n) == BigInteger.ModPow(g, h, n));
        }

        //Вспомогательные функции

        //Нахождение простых множителей числа
        static void SimpleMultipliers(BigInteger t)
        {
            BigInteger i = Factorization.run(t);

            while (t != 1)
            {
                if (t % i == 0)
                {
                    t = t / i;
                }
                else
                {
                    i = Factorization.run(t);
                }
                if (SimplesList.IndexOf(i) < 0)
                    SimplesList.Add(i);
            }
        }
    }
}
