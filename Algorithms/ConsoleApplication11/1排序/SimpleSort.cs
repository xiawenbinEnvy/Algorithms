using System;

namespace ConsoleApplication11
{
    /// <summary>
    /// 选择排序
    /// </summary>
    class SelectionSort
    {
        public void Sort(IComparable[] a)
        {
            int l = a.Length;
            for (int i = 0; i < l; i++)
            {
                int min = i;
                for (int j = i + 1; j < l; j++)
                {
                    if (a[j].CompareTo(a[min]) < 0)
                    {
                        min = j;
                    }
                }
                Exchange(a, i, min);
            }
        }

        private void Exchange(IComparable[] a, int i, int j)
        {
            IComparable tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;
        }
    }
    /// <summary>
    /// 插入排序
    /// </summary>
    class InsertionSort
    {
        public void Sort(IComparable[] a)
        {
            int l = a.Length;
            for (int i = 1; i < l; i++)
            {
                for (int j = i; j > 0 && (a[j].CompareTo(a[j - 1]) < 0); j--)
                {
                    Exchange(a, j, j - 1);
                }
            }
        }

        private void Exchange(IComparable[] a, int i, int j)
        {
            IComparable tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;
        }
    }
}
