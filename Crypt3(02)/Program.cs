using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Threading;

namespace Crypt3_02_
{
    class Program
    {
        static void Main(string[] args)
        {
            int Nice = 0;
            int NotNice = 0;
            BigIntegerRandom BIR = new BigIntegerRandom();

            DigitalSignature NewSignature = new DigitalSignature(64);//p,g
                                                                     //Console.WriteLine("i=={0,3} G=={1}", i,NewSignature.G);

            BigInteger X;
            do
            {
                X = BIR.GenerateSimple(64);
                if (GCDEX.GetX(X, NewSignature.P - 1) > 0)
                    break;
            } while (true);
            //Закрытый ключ
            BigInteger M = BigInteger.Parse("465498132");//Исходное сообщение
            BigInteger Z = BigInteger.ModPow(M, X, NewSignature.P);//Подписанное сообщение

            for (int i=0; i<10; i++)
                if (Bob(NewSignature.P, NewSignature.G, X, Z, M))
                {
                    Nice++;
                }
                else
                {
                    NotNice++;
                }

            Console.WriteLine("Nice=={0}\nNotNice=={1}", Nice, NotNice);
        }
        /// <summary>
        /// Функция генерирует два случайных числа a и b меньше P, возвращает true в случае подлинности подписи
        /// </summary>
        /// <param name="P">Опубликованное простое число</param>
        /// <param name="G">Порождающий элемент</param>
        /// <param name="X">Закрытый ключ</param>
        /// <param name="Z">Подписанное сообщение</param>
        /// <param name="M">Неподписанное сообщение</param>
        /// <returns></returns>
        static bool Bob(BigInteger P, BigInteger G, BigInteger X, BigInteger Z, BigInteger M)
        {
            BigInteger A = BigIntegerRandom.GenerateRandom(0, P, new Random());
            Thread.Sleep(5);
            BigInteger B = BigIntegerRandom.GenerateRandom(0, P, new Random());
            BigInteger C = (BigInteger.ModPow(BigInteger.ModPow(Z, A, P) * BigInteger.ModPow(G, X * B, P), 1, P));
            BigInteger D_Bob = BigInteger.ModPow(BigInteger.ModPow(M, A, P) * BigInteger.ModPow(G, B, P), 1, P);
            BigInteger D_Alice = Alice(C, P, X);
            return D_Alice == D_Bob;
        }

        static BigInteger Alice(BigInteger C, BigInteger P, BigInteger X)
        {
            BigInteger ReverseX = GCDEX.GetX(X, P - 1);
            if (ReverseX > 0)
            {
                BigInteger T = BigInteger.ModPow(ReverseX, 1, P - 1);
                return BigInteger.ModPow(C, T, P);
            }
            else
            {
                return -1;
            }
        }
    }
}
