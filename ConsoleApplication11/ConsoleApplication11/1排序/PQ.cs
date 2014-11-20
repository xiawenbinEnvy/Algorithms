using System;

namespace ConsoleApplication11
{
    /// <summary>
    /// 二叉堆实现的优先队列
    /// </summary>
    abstract class PQ<Key> where Key : IComparable<Key>
    {
        protected Key[] pq = null;
        protected int N = 0;
        public PQ(int n)
        {
            pq = new Key[n + 1];
        }

        public int size()
        {
            return N;
        }

        public void insert(Key k)
        {
            N++;
            pq[N] = k;
            swim(N);
        }

        protected void exchange(int s, int t)
        {
            var tmp = pq[s];
            pq[s] = pq[t];
            pq[t] = tmp;
        }

        public Key DeletePQHead()
        {
            var result = pq[1];
            exchange(1, N);
            N--;
            pq[N + 1] = default(Key);//垃圾回收

            sink(1);
            return result;
        }

        public bool IsEmpty()
        {
            return N == 0;
        }

        protected void swim(int k)
        {
            while (k > 1 && greater(k / 2, k))
            {
                exchange(k, k / 2);
                k = k / 2;
            }
        }

        protected void sink(int k)
        {
            while (2 * k <= N)
            {
                int j = 2 * k;
                if (j < N && greater(j, j + 1)) j++;
                if (!greater(k, j)) break;
                exchange(k, j);
                k = j;
            }
        }

        protected abstract bool greater(int i, int j);
    }

    class MinPQ<Key> : PQ<Key> where Key : IComparable<Key>
    {
        public MinPQ(int NMAX)
            : base(NMAX)
        {

        }
        protected override bool greater(int i, int j)
        {
            return pq[i].CompareTo(pq[j]) > 0;
        }
    }

    class MaxPQ<Key> : PQ<Key> where Key : IComparable<Key>
    {
        public MaxPQ(int NMAX)
            : base(NMAX)
        {

        }
        protected override bool greater(int i, int j)
        {
            return pq[i].CompareTo(pq[j]) < 0;
        }
    }
}
