using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication11._4字符串.排序
{
    /// <summary>
    /// 按照以下方法使用队列进行高位优先字符串排序——
    /// 为每个盒子设置一个队列，在第一次遍历所有元素时
    /// 将每个元素根据首字母插入到适当的队列中，
    /// 然后将每个子列表排序并合并所有队列的到完整的排序结果。
    /// 这种方法中count[]数组不需要在递归方法内创建 
    /// </summary>
    class MSDWithQueue
    {
        Queue<string> result = new Queue<string>();
        public string[] Main(string[] a)
        {
            Queue<string> q = new Queue<string>();
            foreach (var s in a) q.Enqueue(s);

            Practice5_1_11(q, 0);
            return result.ToArray();
        }
        private string Practice5_1_11(Queue<string> queue, int d)
        {
            if (queue.Count == 1) return queue.Dequeue();
            Dictionary<char, Queue<string>> dic = new Dictionary<char, Queue<string>>();
            while (queue.Count > 0)
            {
                string s = queue.Dequeue();
                if (d >= s.Length) continue;
                char first = s[d];
                if (!dic.ContainsKey(first))
                    dic.Add(first, new Queue<string>());
                dic[first].Enqueue(s);
            }
            dic = dic.OrderBy(t => t.Key).ToDictionary(t => t.Key, t => t.Value);
            foreach (var KV in dic)
            {
                result.Enqueue(Practice5_1_11(KV.Value, d + 1));
            }
            return "";
        }
        private int charAt(string s, int d)
        {
            if (s.Length <= d) return -1;//字符串已结束
            return s[d];
        }
    }

    /// <summary>
    /// 利用高位优先处理大型数组，利用三向切分快速排序处理小型数组
    /// </summary>
    class MSDUnionThreewayStringQuickSort
    {
        private string[] aux = null;
        private int R = 256;

        public MSDUnionThreewayStringQuickSort(string[] a)
        {
            int l = a.Length;
            aux = new string[l];
        }

        public void sort(string[] a)
        {
            MSDSort(a, 0, a.Length - 1, 0);
        }

        private void MSDSort(string[] a, int lo, int hi, int d)
        {
            if (lo >= hi) return;
            if (hi - lo <= 2) { ThreewaySort(a, lo, hi, d); return; }

            int[] count = new int[R + 1];
            int[] position = new int[R + 1];

            for (int i = lo; i <= hi; i++)//计算频率
            {
                int c = charAt(a[i], d);
                count[c + 1]++;
            }
            for (int i = 0; i < R; i++)//计算索引
            {
                position[i + 1] = position[i] + count[i];
            }
            for (int i = lo; i <= hi; i++)//分组
            {
                int c = charAt(a[i], d);
                aux[(position[c + 1]++) + lo] = a[i];
            }
            for (int i = lo; i <= hi; i++)//回写
            {
                a[i] = aux[i];
            }

            for (int i = 0; i < R - 1; i++)
            {
                int newLo = lo + position[i + 1];
                int newHi = lo + position[i + 2] - 1;
                if (newLo >= newHi) continue;
                MSDSort(a, newLo, newHi, d + 1);
            }
        }

        private void ThreewaySort(string[] a, int lo, int hi, int d)
        {
            if (lo >= hi) return;

            int lt = lo; int gt = hi; int i = lo + 1;
            int v = charAt(a[lo], d);

            while (i <= gt)
            {
                int n = charAt(a[i], d);
                if (v > n) exch(a, i++, lt++);
                else if (v < n) exch(a, i, gt--);
                else i++;
            }

            ThreewaySort(a, lo, lt - 1, d);
            if (v >= 0) ThreewaySort(a, lt, gt, d + 1);
            ThreewaySort(a, gt + 1, hi, d);
        }

        private int charAt(string s, int d)
        {
            if (d >= s.Length) return -1;
            return s[d];
        }

        private void exch(string[] a, int i, int j)
        {
            string tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;
        }
    }

    /// <summary>
    /// 利用三向切分快速排序对int值排序
    /// </summary>
    class ThreewayQuickSortOnInt32
    {
        public void sort(int[] a)
        {
            ThreewaySort(a, 0, a.Length - 1);
        }
        private void ThreewaySort(int[] a, int lo, int hi)
        {
            if (lo >= hi) return;

            int lt = lo; 
            int gt = hi; 
            int i = lo + 1;
            int v = a[lo];

            while (i <= gt)
            {
                int n = a[i];
                if (v > n) exch(a, i++, lt++);
                else if (v < n) exch(a, i, gt--);
                else i++;
            }

            ThreewaySort(a, lo, lt - 1);
            //if (v >= 0) ThreewaySort(a, lt, gt, d + 1);
            ThreewaySort(a, gt + 1, hi);
        }

        private void exch(int[] a, int i, int j)
        {
            int tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;
        }
    }

    class Node
    {
        public string key { get; set; }
        public int value { get; set; }
        public Node next { get; set; }
    }
    class ThreewayQuickSortOnLinkedList
    {
        public void sort(List<Node> a)
        {
            sort(a, 0, a.Count - 1, 0);
        }

        private void sort(List<Node> a, int lo, int hi, int d)
        {
            if (lo >= hi) return;
            int lt = lo, gt = hi, i = lo + 1;
            int v = charAt(a.ElementAt(lo).key, d);

            while (i <= gt)
            {
                int n = charAt(a.ElementAt(i).key, d);

                if (v.CompareTo(n) > 0) exch(a, i++, lt++);
                else if (v.CompareTo(n) < 0) exch(a, i, gt--);
                else i++;
            }

            sort(a, lo, lt - 1, d);
            if (v >= 0) sort(a, lt, gt, d + 1);
            sort(a, gt + 1, hi, d);
        }

        private void exch(List<Node> a,int i, int j)
        {
            Node ni = a.ElementAt(i);
            Node nj = a.ElementAt(j);
            string key = ni.key;
            int value = ni.value;
            ni.key = nj.key;
            ni.value = nj.value;
            nj.key = key;
            nj.value = value;
        }

        private int charAt(string s, int d)
        {
            if (d >= s.Length) return -1;
            return s[d];
        }
    }

}
