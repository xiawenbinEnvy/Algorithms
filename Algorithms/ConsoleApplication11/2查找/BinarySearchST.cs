using System;

namespace ConsoleApplication11
{
    /// <summary>
    /// 顺序符号表，基于二分查找的get,put,delete
    /// </summary>
    class BinarySearchST<Key, Value>
        where Key : IComparable<Key>
    {
        class Item
        {
            public Key k { get; set; }
            public Value v { get; set; }
        }

        private Item[] st = null;
        private int N;

        public BinarySearchST()
        {
            st = new Item[2];
            N = 0;
        }

        private int rank(Key k, int lo, int hi)
        {
            if (hi < lo) return lo;//关键——就算没找到，也要返回比key小的元素的个数
            int mid = (lo + hi) / 2;
            if (st[mid].k.CompareTo(k) > 0)
                return rank(k, lo, mid - 1);
            else if (st[mid].k.CompareTo(k) < 0)
                return rank(k, mid + 1, hi);
            else
                return mid;
        }

        private void resize(int l)
        {
            Item[] tmp = new Item[l];
            for (int i = 0; i < N; i++)
                tmp[i] = st[i];
            st = tmp;
        }

        public Value Get(Key k)
        {
            int i = rank(k, 0, N - 1);
            if (i < N && st[i].k.CompareTo(k) == 0)
                return st[i].v;
            return default(Value);
        }

        public void Put(Key k, Value v)
        {
            int i = rank(k, 0, N - 1);
            if (i < N && st[i].k.CompareTo(k) == 0)
            {
                st[i].v = v;
                return;
            }

            if (N == st.Length)
                resize(st.Length * 2);

            for (int j = N; j > i; j--)
                st[j] = st[j - 1];

            st[i].k = k;
            st[i].v = v;
            N++;
            return;
        }

        public void Delete(Key k)
        {
            int i = rank(k, 0, N - 1);
            if (i < N && st[i].k.CompareTo(k) == 0)
            {
                for (int j = i; j < N - 1; j++)
                    st[j] = st[j + 1];
                N--;
                st[N] = null;

                if (N == st.Length / 4)
                    resize(st.Length / 2);
            }
        }
    }
}
