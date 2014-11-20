namespace ConsoleApplication11
{
    /// <summary>
    /// 后缀数组——某字符串的所有位置开始得到的子字符串组成的数组，将该数组排序之后，最长重复子字符串会出现在数组的相邻位置，遍历一遍即可找出最长公共前缀
    /// </summary>
    class SuffixArray
    {
        private string[] suffixes;//后缀数组
        private int N;//字符串的长度（即后缀数组的维度）

        public SuffixArray(string s)
        {
            N = s.Length;
            suffixes = new string[N];
            for (int i = 0; i < N; i++) suffixes[i] = s.Substring(i);//构造后缀数组
            MSD msd = new MSD();//将后缀数组排序
            msd.sort(suffixes);
        }

        /// <summary>
        /// 获取最长重复子字符串
        /// </summary>
        public string GetLRS()
        {
            string lrs = "";
            for (int i = 1; i < N; i++)
            {
                int length = lcp(i);
                if (length > lrs.Length)
                    lrs = suffixes[i].Substring(0, length);
            }
            return lrs;
        }
        /// <summary>
        /// select(i)和select(i-1)的最长公共前缀长度
        /// </summary>
        private int lcp(int i)
        {
            return lcp(suffixes[i], suffixes[i - 1]);
        }
        /// <summary>
        /// 两字符串的最长公共前缀
        /// </summary>
        private int lcp(string s, string t)
        {
            int N = s.Length < t.Length ? s.Length : t.Length;
            for (int i = 0; i < N; i++)
            {
                if (s[i] != t[i]) return i;
            }
            return N;
        }

        /// <summary>
        /// 某子字符串的起始位置在文本中的索引
        /// </summary>
        public int index(string key) 
        {
            int i = rank(key);
            return N - suffixes[i].Length;
        }
        /// <summary>
        /// 二分查找
        /// </summary>
        private int rank(string key)
        {
            int lo = 0; int hi = N - 1;
            while (lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;
                int cmp = key.CompareTo(suffixes[mid]);
                if (cmp < 0) hi = mid - 1;
                else if (cmp > 0) lo = mid + 1;
                else return mid;
            }
            return lo;
        }
    }
}
