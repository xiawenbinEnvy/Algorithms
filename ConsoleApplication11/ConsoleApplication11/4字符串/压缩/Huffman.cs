using System;

namespace ConsoleApplication11
{
    /// <summary>
    /// 霍夫曼树的构造
    /// </summary>
    class Huffman
    {
        int R = 256;
        /// <summary>
        /// 结点
        /// </summary>
        class Node : IComparable<Node>
        {
            public Node left, right;//左右结点，left 0,right 1
            public char ch;//字符
            public int freq;//字符出现频率

            public Node(char ch, int freq, Node left, Node right)
            {
                this.ch = ch;
                this.freq = freq;
                this.left = left;
                this.right = right;
            }

            /// <summary>
            /// 是否是叶子——字符出现在叶子上
            /// </summary>
            public bool isLeaf()
            {
                return left == null && right == null;
            }

            public int CompareTo(Node other)
            {
                return this.freq - other.freq;
            }
        }

        /// <summary>
        /// 压缩
        /// </summary>
        public void compress(string input)
        {
            char[] cinput = input.ToCharArray();
            int[] freq = new int[R];

            //第一步：计算每个字符出现频率
            buildFreq(cinput, freq);

            //第二步：用频率构造霍夫曼树
            Node root = buildTrie(freq);

            //第三步：构造编译表
            string[] st = new string[R];
            buildCode(st, root, "");
        }

        /// <summary>
        /// 计算每个字符出现的频率
        /// </summary>
        private void buildFreq(char[] cinput, int[] freq)
        {
            for (int i = 0; i < cinput.Length; i++)
                freq[cinput[i]]++;
        }

        /// <summary>
        /// 构造霍夫曼树(频率高的字符离根近，频率低的字符离根远)
        /// </summary>
        private Node buildTrie(int[] freq)
        {
            MinPQ<Node> pq = new MinPQ<Node>(R);
            //初始化优先队列
            for (char c = char.MinValue; c < R; c++)
                if (freq[c] > 0)
                    pq.insert(new Node(c, freq[c], null, null));

            while (pq.size() > 1)
            {
                //合并两棵频率最小的树
                Node x = pq.DeletePQHead();
                Node y = pq.DeletePQHead();

                Node xy = new Node('\0', x.freq + y.freq, x, y);
                pq.insert(xy);
            }

            return pq.DeletePQHead();
        }

        /// <summary>
        /// 构造编译表
        /// </summary>
        private void buildCode(string[] st, Node x, string s)
        {
            if (x.isLeaf())
            {
                st[x.ch] = s;//每个字符的最终编码
                return;
            }
            buildCode(st, x.left, s + '0');
            buildCode(st, x.right, s + '1');
        }
    }
}
