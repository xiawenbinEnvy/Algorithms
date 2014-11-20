namespace ConsoleApplication11
{
    /// <summary>
    /// BoyerMoore子字符串查找算法
    /// </summary>
    class BoyerMoore
    {
        private int[] right;//跳跃表
        private string pat;
        public BoyerMoore(string pat)
        {
            this.pat = pat;
            int R = 256;//字母表基数
            right = new int[R];
            for (int i = 0; i < R; i++) right[i] = -1;
            for (int j = 0; j < pat.Length; j++) right[pat[j]] = j;//计算模式内每个字符出现的最后位置  
        }
        //findinahaystackneedle    needle
        public int search(string txt)
        {
            int N = txt.Length;
            int M = pat.Length;
            int skip;
            for (int i = 0; i <= N - M; i += skip)
            {
                skip = 0;
                for (int j = M - 1; j >= 0; j--)
                {
                    if (txt[i + j] != pat[j])
                    {
                        skip = j - right[txt[i + j]];//若pat内无此字符，right[]为-1，则右移j+1位;否则，把pat内的这个字符和txt当前比较位置对齐，减少比较数
                        if (skip < 1) skip = 1;//最起码txt能移动一位
                        break;
                    }
                }
                if (skip == 0) return i;//找到匹配
            }
            return -1;
        }
    }

    /// <summary>
    /// 暴力字符串查找
    /// </summary>
    class StringSearch
    {
        public int search(string txt, string pat)
        {
            int j, M = pat.Length;
            int i, N = txt.Length;
            for (i = 0, j = 0; i < N && j < M; i++)//i跟踪文本 //j跟踪模式
            {
                if (txt[i] == pat[j]) j++;
                else { i = i - j; j = 0; }//将j重新指回0，i需要回退j位，指向匹配模式第0位的位置
            }
            if (j == M) return i - M;
            return -1;
        }
    }
}
