using System;
using System.Collections.Generic;

namespace ConsoleApplication11
{
    /// <summary>
    /// 三向单词查找树
    /// </summary>
    class TST<Value> where Value:class
    {
        private int N;
        private Node root;//和R向的区别是，三向从root开始就是正式字符
        class Node
        {
            public char c;//字符
            public Node left, middle, right;//三个方向的链接,left指向所有首字母小于本节点的键，right指向所有首字母大于本节点的键，middle指向所有首字母为本节点的键
            public Value val;
        }

        /// <summary>
        /// 获取单词所对应的value
        /// </summary>
        public Value get(string key)
        {
            Node x = get(root, key, 0);
            if (x == null) return default(Value);
            return x.val;
        }
        private Node get(Node x, string pre, int d)
        {
            if (x == null) return null;
            char c = pre[d];
            if (x.c > c) return get(x.left, pre, d);
            else if (x.c < c) return get(x.right, pre, d);
            else if (d < pre.Length - 1) return get(x.middle, pre, d + 1);
            else return x;
        }

        /// <summary>
        /// 插入单词
        /// </summary>
        public void put(string key, Value val)
        {
            if (string.IsNullOrEmpty(key)) return;
            root = put(root, key, val, 0);
        }
        private Node put(Node x, string key, Value val, int d)
        {
            char c = key[d];
            if (x == null)
            {
                x = new Node();
                x.c = c;
            }
            if (x.c > c) x.left = put(x.left, key, val, d);
            else if (x.c < c) x.right = put(x.right, key, val, d);
            else if (d < key.Length - 1) x.middle = put(x.middle, key, val, d + 1);
            else
            {
                if (x.val == null) N++;
                x.val = val;
            }
            return x;
        }
        public int size()
        {
            return N;
        }
        /// <summary>
        /// 获取单词树中的所有单词
        /// </summary>
        public IEnumerable<string> keys()
        {
            Queue<string> q = new Queue<string>();
            collect(root, "", q);
            return q;
        }
        /// <summary>
        /// 前缀匹配
        /// </summary>
        public IEnumerable<string> keysWithPrefix(string pre)
        {
            Node x = get(root, pre, 0);
            Queue<string> q = new Queue<string>();
            if (x == null) return q;
            if (x.val != null) q.Enqueue(pre);
            collect(x.middle, pre, q);//从middle下去，因为比如sh，left的话可能是se啥啥的，只有middle下去才是shells之类的
            return q;
        }
        /// <summary>
        /// 通配符匹配——.he匹配she or the ，s..匹配she or sea
        /// </summary>
        public IEnumerable<string> keysThatMath(string pat)
        {
            Queue<String> queue = new Queue<String>();
            collect(root, "", 0, pat, queue);
            return queue;
        }

        private void collect(Node x, string prefix, Queue<string> queue)
        {
            if (x == null) return;
            collect(x.left, prefix, queue);
            if (x.val != null) queue.Enqueue(prefix + x.c);//val只会出现在中间那条线上
            collect(x.middle, prefix + x.c, queue);//沿中间往下的话需要跟踪字符
            collect(x.right, prefix, queue);
        }

        private void collect(Node x, string prefix, int i, string pat, Queue<string> q)
        {
            if (x == null) return;
            char c = pat[i];
            if (c == '.' || c < x.c) collect(x.left, prefix, i, pat, q);
            if (c == '.' || c == x.c)
            {
                if (i == pat.Length - 1 && x.val != null) q.Enqueue(prefix + x.c);
                if (i < pat.Length - 1) collect(x.middle, prefix + x.c, i + 1, pat, q);
            }
            if (c == '.' || c > x.c) collect(x.right, prefix, i, pat, q);
        }
    }
}
