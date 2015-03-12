using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication11._4字符串.子字符串操作
{
    class Practice
    {
        /// <summary>
        /// 计算字符串中连续M个空格第一次出现的位置
        /// </summary>
        public int Practice5_3_4(string txt, int M)
        {
            int N = txt.Length;
            int skip = 0;
            for (int i = 0; i <= N - M; i += skip)
            {
                skip = 0;
                for (int j = M - 1; j >= 0; j--)
                {
                    if (txt[i + j] != ' ')
                    {
                        skip = j + 1;
                        break;
                    }
                }
                if (skip == 0) return i;
            }
            return -1;
        }

        /// <summary>
        /// 基于暴力子字符串查找算法，统计模式字符串在文本中出现的次数以及位置列表
        /// </summary>
        public int Practice5_3_7(string txt, string pat)
        {
            List<int> positionList = new List<int>();
            int count = 0;
            for (int i = 0; i <= txt.Length - pat.Length; i++)
            {
                int position = Practice5_3_7(txt, pat, i);
                if (position > -1)
                {
                    count++;
                    positionList.Add(position);
                }
            }
            return count;
        }
        private int Practice5_3_7(string txt, string pat, int low)
        {
            int N = txt.Length;
            int M = pat.Length;
            int j = 0;
            for (int i = low; i < N && j < M; i++)
            {
                if (txt[i] == pat[j]) j++;
                else return -1;
            }
            if (j == M) return low;
            return -1;
        }

        /// <summary>
        /// 基于BoyerMoore算法，统计模式字符串在文本中出现的次数以及位置列表
        /// </summary>
        public int Practice5_3_9(string txt, string pat)
        {
            List<int> positionList = new List<int>();
            int count = 0;
            int[] right = new int[256];
            for (int i = 0; i < 256; i++) right[i] = -1;
            for (int i = 0; i < pat.Length; i++) right[pat[i]] = i;

            int position;
            for (int i = 0; i < txt.Length - pat.Length; i = position)
            {
                position = Practice5_3_9(txt, pat, i, right);
                if (position > 0)
                {
                    positionList.Add(position - 1);
                    count++;
                }
                else if (position == 0)
                { 
                    return count; 
                }
            }
            return count;
        }
        public int Practice5_3_9(string txt, string pat, int low, int[] right)
        {
            int N = txt.Length;
            int M = pat.Length;
            int skip = 0;
            for (int i = low; i <= N - M; i += skip)
            {
                skip = 0;
                for (int j = M - 1; j >= 0; j--)
                {
                    if (txt[i + j] != pat[j])
                    {
                        skip = j - right[txt[i + j]];
                        if (skip < 1) skip = 1;
                        break;
                    }
                }
                if (skip == 0) return i + 1;
            }
            return 0;
        }

        /// <summary>
        /// 检测两个字符串是否互为回环变位：如example ampleex 基于BoyerMoore算法
        /// </summary>
        public bool Practice5_3_26(string txt1, string txt2)
        {
            if (txt1.Length != txt2.Length) return false;
            int[] left = new int[256];
            for (int i = 0; i < 256; i++) left[i] = 1;
            for (int i = 0; i < txt1.Length; i++) left[txt1[i]] = i;

            int N = txt1.Length;
            txt1 = txt1 + txt1;
            int M = txt1.Length * 2;

            int skip = 0;
            for (int i = 0; i <= N; i += skip)
            {
                skip = 0;
                for (int j = 0; j < N; j++)
                {
                    if (txt1[i + j] != txt2[j])
                    {
                        skip = i + left[txt2[j]];
                        break;
                    }
                }
                if (skip == 0) return true;
            }
            return false;
        }

        /// <summary>
        /// 求最大串联重复的起始位置——在字符串s中，基础字符串b的串联重复就是连续将b至少重复两遍（无重叠）的一个字串 
        /// 基于BoyerMoore算法
        /// </summary>
        public int Practice5_3_27(string txt, string pat)
        {
            int N = txt.Length;
            int M = pat.Length;

            int[] right = new int[256];
            for (int i = 0; i < 256; i++) right[i] = -1;
            for (int i = 0; i < M; i++) right[pat[i]] = i;

            List<int> position = new List<int>();
            int skip = 0;
            for (int i = 0; i <= N - M; i += skip)
            {
                skip = 0;
                for (int j = M - 1; j >= 0; j--)
                {
                    if (txt[i + j] != pat[j])
                    {
                        skip = j - right[txt[i + j]];
                        if (skip < 1) skip = 1;
                        break;
                    }
                }
                if (skip == 0)
                {
                    skip = M;

                    if (position.Count == 0)
                    {
                        position.Add(i);
                    }
                    else
                    {
                        int lastPosition = position[position.Count - 1];
                        if (i - lastPosition == M)
                        {
                            position.Add(i);
                        }
                    }
                }
                else
                {
                    position.Clear();
                }
            }
            if (position.Count >= 2) return position[0];
            return -1;
        }

        /// <summary>
        /// 在二进制字符串上使用BoyerMoore算法——因为二进制只是0和1，所以普通的BM算法效率不高
        /// 需要把二进制中的多个位组合起来计算right[]数组
        /// 假设每次取b位，则right[]数组的长度为2的b次方个——比如b=1，则0和1两种情况，b=2，则为00,01，11,10四种情况，以此类推
        /// </summary>
        public int Practice5_3_35(string txt, string pat)
        {
            int x = 4;
            int b = x;//每4位组合起来
            int i = 1;
            while (b > 0)
            {
                i = 2 * i;
                b--;
            }
            int[] right = new int[i];
            for (int j = 0; j < i; j++) right[j] = -1;
            for (int j = 0; j <= pat.Length - x; j++)
            {
                string s = pat.Substring(j, x);
                //从s计算出二进制转十进制的数字
                int i0 = Convert.ToInt32(s.Substring(3, 1));
                int i1 = Convert.ToInt32(s.Substring(2, 1));
                int i2 = Convert.ToInt32(s.Substring(1, 1));
                int i3 = Convert.ToInt32(s.Substring(0, 1));
                int result = i0 * 1 + i1 * 2 + i2 * 2 * 2 + i3 * 2 * 2 * 2;
                right[result] = j;
            }
            int skip = 0;
            for (int j = 0; j <= txt.Length - pat.Length; j += skip)
            {
                skip = 0;
                int t = 1;
                for (int h = pat.Length - 1; h >= 0; h = h - t)
                {
                    string s = pat.Substring(h - x + 1, x);
                    int i0 = Convert.ToInt32(s.Substring(3, 1));
                    int i1 = Convert.ToInt32(s.Substring(2, 1));
                    int i2 = Convert.ToInt32(s.Substring(1, 1));
                    int i3 = Convert.ToInt32(s.Substring(0, 1));
                    int r1 = i0 * 1 + i1 * 2 + i2 * 2 * 2 + i3 * 2 * 2 * 2;
                    s = txt.Substring(h - x + 1 + j, x);
                    i0 = Convert.ToInt32(s.Substring(3, 1));
                    i1 = Convert.ToInt32(s.Substring(2, 1));
                    i2 = Convert.ToInt32(s.Substring(1, 1));
                    i3 = Convert.ToInt32(s.Substring(0, 1));
                    int r2 = i0 * 1 + i1 * 2 + i2 * 2 * 2 + i3 * 2 * 2 * 2;
                    if (r1 != r2)
                    {
                        int re = right[r1];//pat中目前的四个字符的起始位置
                        skip = re - right[r2];//right[r2]为txt中目前和pat对齐的位置的四个字符在pat中的位置
                        if (skip < 1) skip = 1;
                        break;
                    }
                    else
                    {
                        t = x;
                    }
                }
                if (skip == 0) return j;
            }
            return -1;
        }
    }
}
