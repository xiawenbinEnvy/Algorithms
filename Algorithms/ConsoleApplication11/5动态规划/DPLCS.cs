using System;

namespace Algorithms._5动态规划
{
    /// <summary>
    /// 最长公共子序列问题：
    /// 两个序列共同的子序列中的最长者
    /// 子序列为相同顺序，但不一定连续
    /// </summary>
    class DPLCS
    {
        public void Process()
        {
            string X = "ABCBDAB";//"10010101";
            string Y = "BDCABA";//"010110110";
            int m = X.Length, n = Y.Length;
            int[,] dp = new int[m + 1, n + 1];

            int i = 0;
            int j = 0;
            for (i = 0; i < m; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (X[i] == Y[j]) dp[i + 1, j + 1] = dp[i, j] + 1;
                    else dp[i + 1, j + 1] = Math.Max(dp[i + 1, j], dp[i, j + 1]);
                }
            }

            Print(dp, m, n, X);
            Console.WriteLine(dp[m, n]);
        }

        private void Print(int[,] dp, int i, int j, string X)
        {
            if (i == 0 || j == 0) return;

            if (dp[i, j] == dp[i - 1, j])
                Print(dp, i - 1, j, X);
            else if (dp[i, j] == dp[i, j - 1])
                Print(dp, i, j - 1, X);
            else //if (dp[i, j] == dp[i - 1, j - 1] + 1)
            {
                Print(dp, i - 1, j - 1, X);
                Console.WriteLine(X[i - 1]);
            }
        }
    }
}
