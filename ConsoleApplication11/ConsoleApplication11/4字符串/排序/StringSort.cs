
namespace ConsoleApplication11
{
    /// <summary>
    /// 低位优先的字符串排序（所有字符串长度相等）
    /// 我自己对书上的算法稍微修改了下，更好理解
    /// </summary>
    class LSD
    {
        /// <summary>
        /// W为字符串的长度
        /// </summary>
        public void sort(string[] a, int W)
        {
            int N = a.Length;
            int R = 256;//ASCII字符集的个数

            string[] aux = new string[N];

            for (int d = W - 1; d >= 0; d--)//每个字符
            {
                int[] count = new int[R];
                int[] position = new int[R];
                //计算出现频率
                for (int i = 0; i < N; i++)//每条string
                {
                    count[a[i][d]]++;
                }
                //将频率替换为索引
                for (int r = 0; r < R; r++)//每个字母表
                {
                    if (r == 0) position[r] = 0;
                    else position[r] = count[r - 1] + position[r - 1];
                }
                //将元素分类——即将元素放到他应该在的位置上
                for (int i = 0; i < N; i++)//每条string
                {
                    aux[position[a[i][d]]++] = a[i];
                }
                //回写
                for (int i = 0; i < N; i++)
                {
                    a[i] = aux[i];
                }
            }
        }
    }

    /// <summary>
    /// 高位优先的字符串排序（字符串的长度可以不相等）
    /// 我自己对书上的算法稍微修改了下，更好理解
    /// </summary>
    class MSD
    {
        string[] aux = null;
        int R = 256;//字母表大小
        public void sort(string[] a)
        {
            int N = a.Length;
            aux = new string[N];
            sort(a, 0, N - 1, 0);
        }
        private void sort(string[] a, int lo, int hi, int d)
        {
            if (lo >= hi) return;
            int[] count = new int[R + 1];//因为0位是给已经结束的字符串计数用的，所以要+1
            int[] position = new int[R + 1];

            //计算频率
            for (int i = lo; i <= hi; i++)
            {
                int c = charAt(a[i], d);
                count[c + 1]++;//0为留给已结束的字符串计数，其余的后推一位
            }
            //计算索引
            for (int i = 0; i < R; i++)
            {
                position[i + 1] = position[i] + count[i];//已结束的字符串一定从0位开始，其余的后推一位
            }
            //进行分组
            for (int i = lo; i <= hi; i++)
            {
                int c = charAt(a[i], d);
                aux[lo + (position[c + 1]++)] = a[i];//position是从0位开始，而a[i]是从lo位开始,所以aux要加lo
            }
            //回写
            for (int i = lo; i <= hi; i++)
            {
                a[i] = aux[i];
            }
            //递归对第d个char之后的字符串排序
            for (int i = 0; i < R - 1; i++)
            {
                int newLo = lo + position[i + 1];//i需要加一因为0放的是已结束的字符串的起始位置。本char开始的位置
                int newHi = lo + position[i + 2] - 1;//下一个char开始的位置-1，即本char结束的位置
                if (newLo >= newHi) continue;//优化性能，对只剩一个的字符串不进行排序了，直接跳到下一组
                sort(a, newLo, newHi, d + 1);
            }
        }

        private int charAt(string s, int d)
        {
            if (s.Length <= d) return -1;//字符串已结束
            return s[d];
        }
    }

    /// <summary>
    /// 三向切分的字符串排序——基于三向切分快速排序
    /// </summary>
    class ThreewayStringQuickSort
    {
        public void sort(string[] a)
        {
            sort(a, 0, a.Length - 1, 0);
        }
        private void sort(string[] a, int lo, int hi, int d)
        {
            if (hi <= lo) return;
            int lt = lo;
            int gt = hi;
            int v = charAt(a[lo], d);
            int i = lo + 1;

            while (i <= gt)
            {
                int t = charAt(a[i], d);
                if (t < v) exch(a, i++, lt++);
                else if (t > v) exch(a, i, gt--);
                else i++;
            }

            sort(a, lo, lt - 1, d);
            if (v >= 0) sort(a, lt, gt, d + 1);
            sort(a, gt + 1, hi, d);
        }
        private int charAt(string s, int d)
        {
            if (d < s.Length) return s[d];
            return -1;
        }
        private void exch(string[] a, int i, int j)
        {
            string tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;
        }
    }
}
