
namespace ConsoleApplication11
{
    /// <summary>
    /// KMP子字符串查找算法
    /// </summary>
    class KMP
    {
        private string pat;
        private int[,] dfa;
        public KMP(string pat)
        {
            this.pat = pat;
            int R = 256;
            int M = pat.Length;

            dfa = new int[R, M];
            int X = 0;//重启位置，初始化为0
            dfa[pat[0], 0] = 1;//和第0位匹配，进行到下一位（第1位）

            for (int j = 1; j < M; j++)
            {
                for (int c = 0; c < R; c++)
                    dfa[c, j] = dfa[c, X];//设置匹配失败时的重启位置

                dfa[pat[j], j] = j + 1;//匹配成功时，进入下一位

                X = dfa[pat[j], X];//更新X
            }
        }

        public int search(string txt)
        {
            int i, j, N = txt.Length, M = pat.Length;
            for (i = 0, j = 0; i < N && j < M; i++)
                j = dfa[txt[i], j];

            if (j == M) return i - M;//匹配成功
            return -1;//匹配失败
        }
    }
}
