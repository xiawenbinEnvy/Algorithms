using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.hihoCoder在线题库练习解答
{
    /*
     描述
     小Ho给自己定了一个宏伟的目标：连续100天每天坚持在hihoCoder上提交一个程序。
     100天过去了，小Ho查看自己的提交记录发现有N天因为贪玩忘记提交了。
     于是小Ho软磨硬泡、强忍着小Hi鄙视的眼神从小Hi那里要来M张"补提交卡"。
     每张"补提交卡"都可以补回一天的提交，将原本没有提交程序的一天变成有提交程序的一天。
     小Ho想知道通过利用这M张补提交卡，可以使自己的"最长连续提交天数"最多变成多少天。

     输入
     第一行是一个整数T(1 <= T <= 10)，代表测试数据的组数。

     每个测试数据第一行是2个整数N和M(0 <= N, M <= 100)。
     第二行包含N个整数a1, a2, ... aN(1 <= a1 < a2 < ... < aN <= 100)，表示第a1, a2, ...  aN天小Ho没有提交程序。

     输出
     对于每组数据，输出通过使用补提交卡小Ho的最长连续提交天数最多变成多少。

     样例输入
     3  
     5 1  
     34 77 82 83 84  
     5 2  
     10 30 55 56 90  
     5 10  
     10 30 55 56 90
     样例输出
     76  
     59
     100
     */
    public class AddMissingDay
    {
        public static void Start()
        {
            List<Tuple<int[], int>> data = Prepare();
            List<int> result = new List<int>();

            foreach (var tuple in data)
            {
                int[] missingDays = tuple.Item1;
                int cardCount = tuple.Item2;

                if (cardCount >= missingDays.Length)
                {
                    result.Add(100);
                    continue;
                }

                List<int[]> ranges = Range(missingDays, cardCount);

                int max = -1;
                foreach (var range in ranges)
                {
                    int _max = Calcue(DivideDays(missingDays), range);
                    if (_max > max) max = _max;
                }
                result.Add(max);
            }

            foreach (var i in result) Console.WriteLine(i);
            Console.ReadLine();
        }

        /// <summary>
        /// 从Console中准备数据
        /// </summary>
        private static List<Tuple<int[], int>> Prepare()
        {
            List<Tuple<int[], int>> data = new List<Tuple<int[], int>>();

            int groupCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < groupCount; i++)
            {
                string N_M = Console.ReadLine();
                string[] N_M_Split = N_M.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int N = int.Parse(N_M_Split[0]);
                int M = int.Parse(N_M_Split[1]);

                string Days = Console.ReadLine();
                int[] DaysSplit = Days.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).
                    Select(t => int.Parse(t)).ToArray();

                data.Add(Tuple.Create(DaysSplit, M));
            }

            return data;
        }

        /// <summary>
        /// 区分开提交日和未提交日
        /// </summary>
        private static bool[] DivideDays(int[] darkDay)
        {
            bool[] box = new bool[100];
            for (int i = 1; i <= 100; i++)
            {
                if (darkDay.Any(t => t == i))
                    box[i - 1] = false;
                else
                    box[i - 1] = true;
            }
            return box;
        }

        /// <summary>
        /// 得到连续的可补日期
        /// </summary>
        private static List<int[]> Range(int[] darkDay, int cardCount)
        {
            List<int[]> result = new List<int[]>();
            int[] tmp = null;

            for (int i = 0; i <= darkDay.Length - cardCount; i++)
            {
                tmp = new int[cardCount];
                for (int c = 0; c < cardCount; c++)
                {
                    tmp[c] = darkDay[i + c];
                }
                result.Add(tmp);
            }
            return result;
        }

        /// <summary>
        /// 计算最大连续天数
        /// </summary>
        private static int Calcue(bool[] box, int[] add)
        {
            for (int i = 1; i <= 100; i++)//补提交卡
            {
                if (add.Any(t => t == i)) box[i - 1] = true;
            }

            int lastFalse = -1;
            int max = -1;
            int j = -1;
            int count = 0;

            for (int i = 0; i < 100; i = j)
            {
                if (box[i] == true)
                {
                    j = i + 1;
                    continue;
                }

                for (j = i + 1; j < 100; j++)
                {
                    if (box[j] != false) break;
                }

                count = i - lastFalse - 1;
                if (count > max) max = count;
                lastFalse = j - 1;
            }
            return max;
        }
    }
}
