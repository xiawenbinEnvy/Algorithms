using System;

namespace Algorithms._5动态规划
{
    /// <summary>
    /// 01背包问题
    /// </summary>
    class DP01Bag
    {
        public int Process()
        {
            //物品，最大重量
            int n = 4, W = 5;

            int[] w = new int[n];
            w[0] = 2; w[1] = 1; w[2] = 3; w[3] = 2;
            int[] v = new int[n];
            v[0] = 3; v[1] = 2; v[2] = 4; v[3] = 2;

            int[,] dp = new int[n + 1, W + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= W; j++)
                {
                    if (j < w[i]) dp[i + 1, j] = dp[i, j];
                    else dp[i + 1, j] = Math.Max(dp[i, j], dp[i, j - w[i]] + v[i]);
                }
            }

            return dp[n, W];
        }
    }

    /// <summary>
    /// 完全背包问题
    /// </summary>
    class DPBag
    {
        public int Process()
        {
            //物品，最大重量
            int n = 3, W = 7;

            int[] w = new int[n];
            w[0] = 3; w[1] = 4; w[2] = 2;
            int[] v = new int[n];
            v[0] = 4; v[1] = 5; v[2] = 3;

            int[,] dp = new int[n + 1, W + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= W; j++)
                {
                    for (int k = 0; k * w[i] <= j; k++)
                    {
                        dp[i + 1, j] = Math.Max(dp[i + 1, j], dp[i, j - k * w[i]] + k * v[i]);
                    }
                }
            }

            return dp[n, W];
        }
    }
}
