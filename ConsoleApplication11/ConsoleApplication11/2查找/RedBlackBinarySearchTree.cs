using System;

namespace ConsoleApplication11
{
    /// <summary>
    /// 红黑二叉查找树
    /// </summary>
    class RedBlackBinarySearchTree<Key, Value> where Key : IComparable
    {
        public Node root;

        public class Node
        {
            public bool isRed = false;

            public Node left;

            public Node right;

            public Key k;

            public Value v;

            public Node(Key _k, Value _v, bool _isRed)
            {
                this.k = _k;
                this.v = _v;
                this.isRed = _isRed;
            }
        }

        public Node Get(Key k)
        {
            return Get(root, k);
        }
        private Node Get(Node h, Key k)
        {
            if (h == null) return null;

            int i = h.k.CompareTo(k);
            if (i < 0)
            {
                return Get(h.right, k);
            }
            else if (i > 0)
            {
                return Get(h.left, k);
            }
            else
            {
                return h;
            }
        }

        public void Put(Key k, Value v)
        {
            root = Put(root, k, v);
            root.isRed = false;
        }
        private Node Put(Node h, Key k, Value v)
        {
            if (h == null) return new Node(k, v, true);

            int i = h.k.CompareTo(k);
            if (i < 0)
            {
                h.right = Put(h.right, k, v);
            }
            else if (i > 0)
            {
                h.left = Put(h.left, k, v);
            }
            else
            {
                h.v = v;
            }

            if (IsRed(h.right) && !IsRed(h.left))
            {
                h = TurnLeft(h);
            }
            if (IsRed(h.left) && IsRed(h.left.left))
            {
                h = TurnRight(h);
            }
            if (IsRed(h.left) && IsRed(h.right))
            {
                DeleteColor(h);
            }

            return h;
        }
        private void DeleteColor(Node h)
        {
            h.left.isRed = false;
            h.right.isRed = false;
            h.isRed = true;
        }
        private Node TurnRight(Node h)
        {
            Node x = h.left;
            h.left = x.right;
            x.right = h;
            x.isRed = h.isRed;
            h.isRed = true;

            return x;
        }
        private Node TurnLeft(Node h)
        {
            Node x = h.right;
            h.right = x.left;
            x.left = h;
            x.isRed = h.isRed;
            h.isRed = true;

            return x;
        }
        private bool IsRed(Node node)
        {
            if (node == null) return false;
            return node.isRed;
        }

        /// <summary>
        /// 获取小于等于key的最大Node
        /// </summary>
        public Key floor(Key k)
        {
            Node x = floor(root, k);
            if (x == null) return default(Key);
            return x.k;
        }
        private Node floor(Node x, Key k)
        {
            if (x == null) return null;
            if (k.CompareTo(x.k) == 0) return x;
            if (k.CompareTo(x.k) < 0) return floor(x.left, k);//因为是要获取小于的，所以
            Node t = floor(x.right, k);
            if (t != null) return t;
            else return x;
        }

        private Node ceiling(Node x, Key k)
        {
            if (x == null) return null;
            if (k.CompareTo(x.k) == 0) return x;
            if (k.CompareTo(x.k) < 0)
            {
                Node t = ceiling(x.left, k);
                if (t != null) return t;
                else return x;
            }
            return ceiling(x.right, k);
        }
        /// <summary>
        /// 获取大于等于key的最小Node
        /// </summary>
        public Key ceiling(Key k)
        {
            Node x = ceiling(root, k);
            if (x == null) return default(Key);
            return x.k;
        }

        public void delete(Key k)
        {
            root = delete(root, k);
        }
        private Node delete(Node x, Key k)//这只是普通二叉树的删除，红黑二叉树的删除更复杂
        {
            if (x == null) return null;
            int cmp = k.CompareTo(x.k);
            if (cmp < 0) x.left = delete(x.left, k);
            else if (cmp > 0) x.right = delete(x.right, k);
            else
            {
                if (x.right == null) return x.left;
                if (x.left == null) return x.right;
                Node t = x;
                x = min(t.right);//获取到除了要删除的节点之外，大于要删除节点左子树的最小节点
                x.right = deleteMin(t.right);
                x.left = t.left;
            }
            return x;
        }
        public Node min()
        {
            return min(root);
        }
        //max类似，改成右子树
        private Node min(Node x)
        {
            if (x.left == null) return x;
            return min(x.left);
        }
        /// <summary>
        /// 删除最小
        /// </summary>
        public void deleteMin()
        {
            root = deleteMin(root);
        }

        private Node deleteMin(Node x)
        {
            if (x.left == null)
                return x.right;

            x.left = deleteMin(x.left);
            return x;
        }
    }
}
