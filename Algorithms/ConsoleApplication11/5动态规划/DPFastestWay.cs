using System;

namespace Algorithms._5动态规划
{
    /// <summary>
    /// 装配线问题
    /// </summary>
    class DPFastestWay
    {
        public void Process()
        {
            int i = 2;//两条线路
            int n = 6;//6个站点

            int[,] f = new int[i, n];//记录成本
            int[,] l = new int[i, n - 1];//记录路线

            //e表示进入时耗费的量，x表示流出时耗费的量
            int e1 = 2;
            int e2 = 4;
            int x1 = 3;
            int x2 = 2;

            int[,] a = new int[i, n];
            a[0, 0] = 7;
            a[0, 1] = 9;
            a[0, 2] = 3;
            a[0, 3] = 4;
            a[0, 4] = 8;
            a[0, 5] = 4;

            a[1, 0] = 8;
            a[1, 1] = 5;
            a[1, 2] = 6;
            a[1, 3] = 4;
            a[1, 4] = 5;
            a[1, 5] = 7;

            int[,] t = new int[i, n - 1];
            t[0, 0] = 2;
            t[0, 1] = 3;
            t[0, 2] = 1;
            t[0, 3] = 3;
            t[0, 4] = 4;
            t[1, 0] = 2;
            t[1, 1] = 1;
            t[1, 2] = 2;
            t[1, 3] = 2;
            t[1, 4] = 1;

            int end = -1;
            f[0, 0] = e1 + a[0, 0];
            f[1, 0] = e2 + a[1, 0];
            for (int j = 1; j < n; j++)
            {
                int f1_j = f[0, j - 1] + a[0, j];
                int f2_1_j = f[1, j - 1] + a[0, j] + t[1, j - 1];
                if (f1_j <= f2_1_j)
                {
                    f[0, j] = f1_j;
                    l[0, j - 1] = 0;
                }
                else
                {
                    f[0, j] = f2_1_j;
                    l[0, j - 1] = 1;
                }

                int f2_j = f[1, j - 1] + a[1, j];
                int f1_2_j = f[0, j - 1] + a[1, j] + t[0, j - 1];
                if (f2_j <= f1_2_j)
                {
                    f[1, j] = f2_j;
                    l[1, j - 1] = 1;
                }
                else
                {
                    f[1, j] = f1_2_j;
                    l[1, j - 1] = 0;
                }
            }
            int min = 0;
            if (f[0, n - 1] + x1 <= f[1, n - 1] + x2)
            {
                min = f[0, n - 1] + x1;
                end = 0;
            }
            else
            {
                min = f[1, n - 1] + x2;
                end = 1;
            }

            Console.WriteLine(min);

            string format = "line {0} station {1}";
            Console.WriteLine(string.Format(format, end, n - 1));
            for (int z = n - 1; z >= 1; z--)
            {
                end = l[end, z - 1];
                Console.WriteLine(string.Format(format, end, z - 1));
            }
        }
    }
}
