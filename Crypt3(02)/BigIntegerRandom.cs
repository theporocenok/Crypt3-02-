using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;

namespace System
{
    class BigIntegerRandom
    {
        private static int[] smallsimple;

        public BigIntegerRandom()
        {
            smallsimple=TakeSimplesFromFile();
        }

        //Генерация случайного целого значения в промежутке от start до end
        public static BigInteger GenerateRandom(BigInteger start, BigInteger end, Random rand)
        {
            BigInteger bi;

            while (true)
            {
                byte[] randomBytesBuf = new byte[rand.Next(1, 17)];
                rand.NextBytes(randomBytesBuf);
                bi = new BigInteger(randomBytesBuf);

                if (bi > start && bi < end)
                {
                    break;
                }
            }
            return bi;
        }
        public BigInteger GenerateSimple(int NeededSize)
        {
            bool check = false;
            BigInteger n = 0;//Большое простое число
            BigInteger F = 0;
            BigInteger R = 0;
            List<BigInteger> SimplesForN = null;
            BigIntegerRandom BIR = new BigIntegerRandom();
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
            return n;
        }

        //Создание числа размера size из случайных чисел
        /// <summary>
        /// Функция генерации случайного числа размера size
        /// </summary>
        /// <param name="size">Длина числа в битах</param>
        /// <param name="Simples">Список простых чисел, из которых сгенерировано число</param>
        /// <returns></returns>
        public BigInteger MakeF(int size, ref List<BigInteger> Simples)
        {
            BigInteger result = 1;
            BigInteger temp = 1;
            int smallSimpleCount = smallsimple.Length - 1;
            int maxDegree = 15;
            Random rnd = new Random();
            //Пока число не достигло необходимой длины бит
            while (Len(result) < size)
            {
                //Выбираем случайное простое число
                int SmallSimple = smallsimple[rnd.Next(smallSimpleCount)];
                //Выбираем случайную степень
                int Degree = rnd.Next(maxDegree);
                //Возводим простое числов в случайную степень
                BigInteger NumInDegree = BigInteger.Pow(SmallSimple, Degree);

                temp = result * NumInDegree;
                if (Len(temp) <= size)
                {
                    result = temp;
                    //Проверка на вхождение простого числа в список уже имеющихся
                    if (Simples.IndexOf(new BigInteger(SmallSimple))<0)
                    {
                        Simples.Add(new BigInteger(SmallSimple));
                    }
                    /*if (!SingleEntry(SmallSimple,Qs))
                    {
                        Array.Resize<int>(ref Qs, Qs.Length + 1);
                        Qs[Qs.Length - 1] = SmallSimple;
                    }*/
                }
                else
                    //Уменьшаем количество простых чисел для выбора (для ускория поиска)
                    smallSimpleCount = (smallSimpleCount > 1) ? smallSimpleCount - 1 : 1;
            }
            return result;
        }
        public BigInteger MakeF(int size)
        {
            BigInteger result = 1;
            BigInteger temp = 1;
            int smallSimpleCount = smallsimple.Length - 1;
            int maxDegree = 15;
            Random rnd = new Random();
            //Пока число не достигло необходимой длины бит
            while (Len(result) < size)
            {
                //Выбираем случайное простое число
                int SmallSimple = smallsimple[rnd.Next(smallSimpleCount)];
                //Выбираем случайную степень
                int Degree = rnd.Next(maxDegree);
                //Возводим простое числов в случайную степень
                BigInteger NumInDegree = BigInteger.Pow(SmallSimple, Degree);

                temp = result * NumInDegree;
                if (Len(temp) <= size)
                    result = temp;
                else
                    //Уменьшаем количество простых чисел для выбора (для ускория поиска)
                    smallSimpleCount = (smallSimpleCount > 1) ? smallSimpleCount - 1 : 1;
            }
            return result;
        }
        public BigInteger MakeF(int size,Random rnd)
        {
            BigInteger result = 1;
            BigInteger temp = 1;
            int smallSimpleCount = smallsimple.Length - 1;
            int maxDegree = 15;
            //Пока число не достигло необходимой длины бит
            while (Len(result) < size)
            {
                //Выбираем случайное простое число
                int SmallSimple = smallsimple[rnd.Next(smallSimpleCount)];
                //Выбираем случайную степень
                int Degree = rnd.Next(maxDegree);
                //Возводим простое числов в случайную степень
                BigInteger NumInDegree = BigInteger.Pow(SmallSimple, Degree);

                temp = result * NumInDegree;
                if (Len(temp) <= size)
                    result = temp;
                else
                    //Уменьшаем количество простых чисел для выбора (для ускория поиска)
                    smallSimpleCount = (smallSimpleCount > 1) ? smallSimpleCount - 1 : 1;
            }
            return result;
        }

        //Вспомогательные функции

        //Нахождение битовой длины числа s
        private static int Len(BigInteger s)
        {
            int counter = 0;
            for (int ctr = 0; ; ctr++)
            {
                BigInteger newNumber = s >> ctr;
                if (newNumber == 0)
                    break;
                counter++;
            }
            return counter;
        }

        //Проверка числа на вхождение в массив
        private static bool SingleEntry(int Num, int[] Qs)
        {
            foreach (int num in Qs)
                if (num == Num)
                    return true;
            return false;
        }

        //Заполнение массива простыми числами из файла
        private static int[] TakeSimplesFromFile()
        {
            string []sNumbers;
            using (StreamReader sr = new StreamReader("Simples.txt"))
            {
                sNumbers = sr.ReadToEnd().Split(',');
            }
            int[] Numbers = new int[sNumbers.Length];
            for (int i = 0; i < sNumbers.Length; i++)
                Numbers[i] = int.Parse(sNumbers[i]);
            return Numbers;
        }
    }
}
