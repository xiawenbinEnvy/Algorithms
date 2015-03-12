using System.Collections.Generic;

namespace ConsoleApplication11
{
    /// <summary>
    /// R向单词查找树
    /// </summary>
    class TrieST<Value>
    {
        //符号表基数
        private static int R = 256;//更大的字母表的话，就不建议使用R向Trie了，因为空间实在负担不起

        class Node
        {
            public Value value;
            public Node[] Next = new Node[R];//每个节点会有256个后继节点，其中大多数将会为null
        }

        private Node root;//根节点

        /// <summary>
        /// 获取单词所对应的value
        /// </summary>
        public Value get(string key)
        {
            Node x = get(root, key, 0);
            if (x == null) return default(Value);
            return x.value;
        }
        private Node get(Node x, string key, int d)
        {
            if (x == null) return null;//未查找到
            if (d == key.Length) return x;//要查找的字符串结束，先不论这个node有无value
            char c = key[d];
            return get(x.Next[c], key, d + 1);
        }

        /// <summary>
        /// 树中值的数量
        /// </summary>
        public int size()
        {
            return size(root);
        }
        private int size(Node x)
        {
            if (x == null) return 0;
            int cnt = 0;
            if (!x.value.Equals(default(Value))) cnt++;
            for (char c = char.MinValue; c < R; c++)
            {
                if (x.Next[c] == null) continue;//增加性能
                cnt += size(x.Next[c]);
            }

            return cnt;
        }
        /// <summary>
        /// 插入单词
        /// </summary>
        public void put(string key, Value value)
        {
            root = put(root, key, value, 0);
        }
        private Node put(Node x, string key, Value value, int d)
        {
            if (x == null) x = new Node();
            if (d == key.Length)
            {
                x.value = value;
                return x;
            }
            char c = key[d];
            x.Next[c] = put(x.Next[c], key, value, d + 1);
            return x;
        }
        public bool isEmpty()
        {
            if (root == null) return true;
            return isEmpty(root);
        }
        private bool isEmpty(Node x)
        {
            for (char c = char.MinValue; c < R; c++)
            {
                if (x.Next[c] != null && !x.Next[c].value.Equals(default(Value)))
                    return false;
            }
            return true;
        }
        public bool contains(string key)
        {
            return contains(root, key, 0);
        }
        private bool contains(Node x, string key, int d)
        {
            if (x == null) return false;
            if (d == key.Length && !x.value.Equals(default(Value))) return true;
            char c = key[d];
            return contains(x.Next[c], key, d + 1);
        }
        /// <summary>
        /// 获取单词树中的所有单词键
        /// </summary>
        public IEnumerable<string> keys()
        {
            Queue<string> q = new Queue<string>();
            collect(root, "", q);
            return q;
        }
        /// <summary>
        /// 前缀匹配（所有以pre开头的键）
        /// </summary>
        public IEnumerable<string> keysWithPrefix(string pre)
        {
            Queue<string> q = new Queue<string>();
            Node preNode = get(root, pre, 0);//先找到pre最后一个字符对应的节点
            collect(preNode, pre, q);//从这个节点开始收集拥有值的键字符串
            return q;
        }
        private void collect(Node x, string pre, Queue<string> q)
        {
            if (x == null) return;
            if (!x.value.Equals(default(Value))) q.Enqueue(pre);
            for (char c = char.MinValue; c < R; c++)
            {
                if (x.Next[c] == null) continue;//增加性能
                collect(x.Next[c], pre + c.ToString(), q);
            }
        }

        /// <summary>
        /// 通配符匹配——.he匹配she or the ，s..匹配she or sea
        /// </summary>
        public IEnumerable<string> keysThatMath(string pat)
        {
            Queue<string> q = new Queue<string>();
            collect(root, "", pat, q);
            return q;
        }
        /// <param name="pre">已走过的字符拼成的字符串</param>
        /// <param name="pat">需要匹配的模式，比如s,s.,s..等</param>
        private void collect(Node x, string pre, string pat, Queue<string> q)
        {
            //.he匹配she or the ，s..匹配she or sea
            if (x == null) return;
            int d1 = pre.Length;
            int d2 = pat.Length;
            if (!x.value.Equals(default(Value)) && d1 == d2) q.Enqueue(pre);//不光需要有value，还需要和通配符匹配的长度完全一样
            if (d1 == d2) return;//等于通配符匹配的长度了，就不继续往下递归了

            char next = pat[d1];
            for (char c = char.MinValue; c < R; c++)
                if (next == '.' || next == c)//.时，所有后继都查找||查找和通配符匹配的当前字母相同的那个后继
                    collect(x.Next[c], pre + c, pat, q);
        }

        /// <summary>
        /// 最长键前缀
        /// </summary>
        public string longestPrefix(string pre)
        {
            int l = search(root, pre, 0, 0);
            return pre.Substring(0, l);
        }
        /// <summary>
        /// 比如树中有shells、she,最长键前缀shell为she,最长键前缀shellshort为shells
        /// </summary>
        private int search(Node x, string pre, int d, int l)
        {
            if (x == null) return l;//已到最后节点
            if (!x.value.Equals(default(Value))) l = d;//有值，记录下来,作为目前来说最后一个有效长度
            if (d == pre.Length) return l;//已是模式的最后一个字符，结束递归
            char c = pre[d];
            return search(x.Next[c], pre, d + 1, l);
        }

        /// <summary>
        /// 删除键
        /// </summary>
        public void delete(string key)
        {
            root = delete(root, key, 0);
        }
        private Node delete(Node x, string key, int d)
        {
            if (x == null) return null;
            if (d == key.Length)//最后一个字符了
            {
                x.value = default(Value);
            }
            else//继续向下递归
            {
                char c = key[d];
                x.Next[c] = delete(x.Next[c], key, d + 1);
            }

            //只有没有value的节点，才进行后续的是否要删掉节点的判断，提高性能
            if (!x.value.Equals(default(Value))) return x;

            //本节点有后继结点的话，就不需要删除此节点
            for (char c = char.MinValue; c < R; c++)
                if (x.Next[c] != null)
                    return x;

            //否则本节点没有任何后继节点，删除此节点
            return null;
        }
    }
}
