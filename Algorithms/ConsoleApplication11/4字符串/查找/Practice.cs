using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication11._4字符串.查找
{
    class TrieSTOnPractice<Value>
    {
        private static int R = 256;
        class Node
        {
            public Value v;
            public Node[] next = new Node[R];
        }
        private Node root;
        private int N;
        public void put(string key, Value v)
        {
            root = put(root, key, v, 0);
        }
        private Node put(Node x, string key, Value v, int d)
        {
            if (x == null) x = new Node();
            if (key.Length == d)
            {
                if (x.v.Equals(default(Value))) N++;
                x.v = v;
                return x;
            }
            char c = key[d];
            x.next[c] = put(x.next[c], key, v, d + 1);
            return x;
        }
        public int size()
        {
            return N;
        }
        public string floor(string key)
        {
            StringBuilder pre = new StringBuilder();
            floor(root, pre, key, 0);
            return pre.ToString();
        }
        private void floor(Node x, StringBuilder pre, string key, int d)
        {
            if (x == null) return;
            if (!x.v.Equals(default(Value))) pre.Append(key[pre.Length]);
            if (key.Length == d) return;
            char c = key[d];
            floor(x.next[c], pre, key, d + 1);
        }

        private Node get(Node x, string key, int d)
        {
            if (x == null) return null;
            if (d == key.Length) return x;
            char c = key[d];
            return get(x.next[c], key, d + 1);
        }
        private void collect(Node x, string pre, Queue<string> q)
        {
            if (x == null) return;
            if (!x.v.Equals(default(Value))) q.Enqueue(pre);
            for (char c = char.MinValue; c < R; c++)
            {
                if (x.next[c] == null) continue;
                collect(x.next[c], pre + c, q);
            }
        }
        public string ceiling(string key)
        {
            Node x = get(root, key, 0);
            if (x == null) return "";
            if (!x.v.Equals(default(Value))) return key;
            Queue<string> q = new Queue<string>();
            collect(x, key, q);
            return q.OrderBy(t => t.Length).First();
        }
    }

    class TrieSTOnRandomTelNumber
    {
        private static int R = 10;
        List<string> QH = new List<string>()
        {
            "000","001","002","003","004","005","006","007","008","009"
        };
        class Node
        {
            public int value;
            public Node[] next = new Node[R];
        }
        private Node root;
        private int N;
        public void Build(int M)
        {
            while (N < M)
            {
                put(1);
            }
        }
        private void put(int v)
        {
            Random rd = new Random();
            int i = rd.Next(0, 10);
            string MyQH = QH[i];//区号

            string HM =
                MyQH +
                rd.Next(0, 10) + rd.Next(0, 10) + rd.Next(0, 10) +
                rd.Next(0, 10) + rd.Next(0, 10) + rd.Next(0, 10) + rd.Next(0, 10);

            root = put(root, HM, 0, v);
        }
        private Node put(Node x, string key, int d,int v)
        {
            if (x == null) x = new Node();
            if (d == key.Length)
            {
                if (x.value.Equals(default(int))) N++;
                x.value = v;
                return x;
            }
            int c = charAt(key, d);
            x.next[c] = put(x.next[c], key, d + 1, v);
            return x;
        }
        private int charAt(string s, int d)
        {
            return Convert.ToInt32(s.Substring(d, 1));
        }
    }

    class CompressTrieST
    {
        private static int R = 256;
        class Node
        {
            public int value;
            public Node[] next = new Node[R];
            public CompressedNode compress;
        }
        class CompressedNode
        {
            public int value;
            public string key;
        }
        private Node root;
        public void put(string key, int v)
        {
            root = put(root, key, 0, v);
            Compress(root);
        }
        private void Compress(Node x)
        {
            for (int i = 0; i < R; i++)
            {
                if (x.next[i] == null) continue;
                if (IsSingleBranchNode(x.next[i]))
                {
                    StringBuilder sb = new StringBuilder();
                    collect(x.next[i], sb);
                    x.next[i].compress = new CompressedNode() { key = sb.ToString(), value = 0 };
                    x.next[i].next = new Node[R];
                }
                else
                {
                    Compress(x.next[i]);
                }
            }
        }
        private Node put(Node x, string key, int d, int v)
        {
            if (x == null) x = new Node();
            if (d == key.Length)
            {
                x.value = v;
                return x;
            }
            int c = key[d];
            x.next[c] = put(x.next[c], key, d + 1, v);
            return x;
        }
        private bool IsSingleBranchNode(Node x)
        {
            return LeafDFS(x) == FloorDFS(x);
        }
        private int LeafDFS(Node x)
        {
            if (x == null) return 0;
            int X = 1;
            for (int i = 0; i < R; i++)
            {
                if (x.next[i] == null) continue;
                X += LeafDFS(x.next[i]);
            }
            return X;
        }
        private int FloorDFS(Node x)
        {
            if (x == null) return 0;
            int X = 1;
            for (int i = 0; i < R; i++)
            {
                if (x.next[i] == null) continue;
                X += FloorDFS(x.next[i]);
                break;
            }
            return X;
        }
        private void collect(Node x, StringBuilder pre)
        {
            if (x == null) return;
            if (!x.value.Equals(default(int))) return;
            for (char i = char.MinValue; i < R; i++)
            {
                if (x.next[i] == null) continue;
                pre.Append(i);
                collect(x.next[i], pre);
            }
        }
    }

    class SpellChecker
    {
        class TST<Value>
        {
            class Node
            {
                public char c;
                public Value v;
                public Node left, mid, right;
            }
            private Node root;

            public void put(string key, Value value)
            {
                root = put(root, key, 0, value);
            }
            private Node put(Node x, string key, int d, Value v)
            {
                char c = key[d];
                if (x == null)
                {
                    x = new Node();
                    x.c = c;
                }
                if (c < x.c)
                    x.left = put(x.left, key, d, v);
                else if (c > x.c)
                    x.right = put(x.right, key, d, v);
                else if (d < key.Length - 1)
                    x.mid = put(x.mid, key, d + 1, v);
                else
                    x.v = v;
                return x;
            }

            public Value get(string key)
            {
                Node x = get(root, key, 0);
                if (x == null) return default(Value);
                return x.v;
            }
            private Node get(Node x, string key, int d)
            {
                if (x == null) return null;
                char c = key[d];
                if (c < x.c)
                    return get(x.left, key, d);
                else if (c > x.c)
                    return get(x.right, key, d);
                else if (d < key.Length - 1)
                    return get(x.mid, key, d + 1);
                else return x;
            }
        }

        private TST<int> tst;
        public SpellChecker()
        {
            tst = new TST<int>();
        }

        public void insert(string key, int pageindex)
        {
            tst.put(key, pageindex);
        }

        public IEnumerable<string> GetExistedNotWord(IEnumerable<string> l)
        {
            Queue<string> q = new Queue<string>();
            foreach (var s in l)
            {
                int i = tst.get(s);
                if (i == 0) q.Enqueue(s);
            }
            return q;
        }
    }
}
