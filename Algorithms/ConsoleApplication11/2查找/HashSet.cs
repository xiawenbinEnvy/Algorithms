
namespace ConsoleApplication11
{
    /// <summary>
    /// 基于拉链法的散列表
    /// </summary>
    class SeqarateChainHashST<Key, Value>
    {
        /// <summary>
        /// 无序链表
        /// </summary>
        public class SequentialSearchST
        {
            private class Node
            {
                public Key k { get; set; }
                public Value v { get; set; }
                public Node next { get; set; }
                public Node(Key k, Value v, Node next)
                {
                    this.k = k; this.v = v; this.next = next;
                }
            }

            private Node first;

            public Value Get(Key k)
            {
                for (Node x = first; x != null; x = x.next)
                {
                    if (x.k.Equals(k))
                    {
                        return x.v;
                    }
                }
                return default(Value);
            }

            public void Put(Key k, Value v)
            {
                for (Node x = first; x != null; x = x.next)
                {
                    if (x.k.Equals(k))
                    {
                        x.v = v; return;
                    }
                }

                first = new Node(k, v, first);
            }

            public void Delete(Key k)
            {
                delete(first, k);
            }
            private Node delete(Node x, Key key)
            {
                if (x == null) return null;
                if (key.Equals(x.k)) { return x.next; }
                x.next = delete(x.next, key);
                return x;
            }
        }

        private SequentialSearchST[] st = null;//散列数组
        private int M;//散列表大小

        public SeqarateChainHashST()
        {
            this.M = 97;//M越大，每个链表节点越少。97是一个平衡的数字
            st = new SequentialSearchST[M];
            for (int i = 0; i < st.Length; i++)
                st[i] = new SequentialSearchST();
        }

        private int hash(Key k)
        {
            return (k.GetHashCode() & 0x7fffffff) % M;//去除正负号之后对M取余。
        }
        public Value Get(Key k)
        {
            return st[hash(k)].Get(k);
        }

        public void Put(Key k, Value v)
        {
            st[hash(k)].Put(k, v);
        }

        public void Delete(Key k)
        {
            st[hash(k)].Delete(k);
        }
    }

    /// <summary>
    /// 基于线性探测法的散列表
    /// </summary>
    class LinearProbingHashST<Key, Value>
    {
        private int M = 16;//数组大小
        private int N = 0;//散列表中键值对总数

        private Key[] keys = null;
        private Value[] values = null;

        public LinearProbingHashST(int M)
        {
            this.M = M;
            keys = new Key[M];
            values = new Value[M];
        }

        private int hash(Key k)
        {
            return (k.GetHashCode() & 0x7fffffff) % M;//去除正负号之后对M取余。
        }
        public void Put(Key k, Value v)
        {
            if (N >= M / 2) resize(2 * M);//是为了保证绝大多数数组key为null，减少碰撞发生频率，在半满时就扩大数组

            int i = 0;
            for (i = hash(k); keys[i] != null; i = (i + 1) % M)
            {
                if (keys[i].Equals(k))
                {
                    values[i] = v;
                    return;
                }
            }

            keys[i] = k;
            values[i] = v;
            N++;
        }

        public Value Get(Key k)
        {
            for (int i = hash(k); keys[i] != null; i = (i + 1) % M)
                if (keys[i].Equals(k)) return values[i];

            return default(Value);
        }

        public void Delete(Key k)
        {
            bool contains = false;
            for (int j = 0; j < M; j++)
            {
                if (keys[j].Equals(k))
                {
                    contains = true;
                    break;
                }
            }
            if (!contains) return;

            int i = hash(k);
            while (!keys[i].Equals(k)) i = (i + 1) % M;
            keys[i] = default(Key);
            values[i] = default(Value);
            i = (i + 1) % M;
            while (keys[i] != null)
            {
                Key tmpK = keys[i];
                Value tmpV = values[i];
                keys[i] = default(Key);
                values[i] = default(Value);
                N--;
                Put(tmpK, tmpV);//☆
                i = (i + 1) % M;
            }
            N--;

            if (N == M / 8) resize(M / 2);
        }

        /// <summary>
        /// 对于线性探测散列表，调整大小是必须的，因为随着数组非null空间越来越少，
        /// 碰撞发生的几率会增加，并且散列表在填满时会进入无限循环
        /// </summary>
        private void resize(int p)
        {
            LinearProbingHashST<Key, Value> tmp = new LinearProbingHashST<Key, Value>(p);
            for (int i = 0; i < M; i++)
                if (keys[i] != null)
                    tmp.Put(keys[i], values[i]);
            keys = tmp.keys;
            values = tmp.values;
            M = tmp.M;
        }
    }
}
