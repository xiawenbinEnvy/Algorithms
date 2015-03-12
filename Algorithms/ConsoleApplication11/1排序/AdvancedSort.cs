using System;

namespace ConsoleApplication11
{
    /// <summary>
    /// 快速排序
    /// </summary>
    class QuickSort
    {
        /// <summary>
        /// 切分
        /// </summary>
        private int Partition(IComparable[] a, int low, int high)
        {
            IComparable v = a[low];
            int i = low + 1;
            int j = high;
            while (true)
            {
                while (a[i].CompareTo(v) < 0)//要找到左侧大于切分元素的
                {
                    if (i == high) break;
                    i++;
                }
                while (a[j].CompareTo(v) > 0)//要找到右侧小于切分元素的
                {
                    if (j == low) break;
                    j--;
                }
                if (i >= j) break;
                Exchange(a, i, j);
                i++; j--;
            }
            Exchange(a, low, j);//将切分元素放到它应该在的位置，此位置左侧都小于切分元素，右侧都大于切分元素
            return j;
        }

        private void Exchange(IComparable[] a, int i, int j)
        {
            IComparable tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;
        }

        private void Sort(IComparable[] a, int low, int high)
        {
            if (low >= high) return;
            //在小数组时切换到插入排序，增进快排的性能
            //if (low + 15 > -high)
            //{
            //    InsertSort.sort(a,low,high);return;
            //}
            int j = Partition(a, low, high);
            Sort(a, low, j - 1);
            Sort(a, j + 1, high);
        }

        public void Sort(IComparable[] a)
        {
            Random r = new Random();//伪代码:r.Randow(a);用意：需要将数组打乱，基本有序的数组会降低快速排序的性能
            Sort(a, 0, a.Length - 1);
        }
    }

    /// <summary>
    /// 归并排序
    /// </summary>
    class MergeSort
    {
        IComparable[] aux = null;

        public void sort(IComparable[] a)
        {
            aux = new IComparable[a.Length];
            Sort(a, 0, a.Length - 1);
        }

        /// <summary>
        /// 归并
        /// </summary>
        private void Merge(IComparable[] a, int low, int mid, int high)
        {
            for (int p = low; p <= high; p++)
                aux[p] = a[p];

            int i = low; int j = mid + 1;

            for (int k = low; k <= high; k++)
            {
                if (i > mid) a[k] = aux[j++];
                else if (j > high) a[k] = aux[i++];
                else if (aux[j].CompareTo(aux[i]) < 0) a[k] = aux[j++];
                else if (aux[i].CompareTo(aux[j]) < 0) a[k] = aux[i++];
            }
        }

        private void Sort(IComparable[] a, int low, int high)
        {
            if (low >= high) return;
            int mid = low + (high - low) / 2;
            Sort(a, low, mid);
            Sort(a, mid + 1, high);
            Merge(a, low, mid, high);
        }
    }
}
