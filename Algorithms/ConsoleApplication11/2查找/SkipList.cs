using System;
using System.Collections.Generic;

namespace Algorithms._2查找
{
    /// <summary>
    /// 跳表
    /// </summary>
    class SkipList<TKey, TValue> where TKey : IComparable
    {
        private SkipListNode<TKey, TValue> head;

        private int count;

        public SkipList()
        {
            this.head = new SkipListNode<TKey, TValue>();
            count = 0;
        }

        private class SkipListNode<TNKey, TNValue>
        {
            public SkipListNode<TNKey, TNValue> forward, back, up, down;
            public SkipListKVPair<TNKey, TNValue> keyValue;
            public bool isFront = false;

            public TNKey key
            {
                get { return keyValue.Key; }
            }
            public TNValue value
            {
                get { return keyValue.Value; }
                set { keyValue.Value = value; }
            }

            public SkipListNode()
            {
                this.keyValue = new SkipListKVPair<TNKey, TNValue>(default(TNKey), default(TNValue));
                this.isFront = true;
            }

            public SkipListNode(SkipListKVPair<TNKey, TNValue> keyValue)
            {
                this.keyValue = keyValue;
            }

            public SkipListNode(TNKey key, TNValue value)
            {
                this.keyValue = new SkipListKVPair<TNKey, TNValue>(key, value);
            }
        }

        private struct SkipListKVPair<TNKey, TNValue>
        {
            private TNKey key;
            public TNKey Key
            {
                get { return key; }
            }
            public TNValue Value;

            public SkipListKVPair(TNKey key, TNValue value)
            {
                this.key = key;
                this.Value = value;
            }
        }

        public TValue Get(TKey key)
        {
            SkipListNode<TKey, TValue> position = search(key);
            if (position == null || position.isFront) throw new KeyNotFoundException("Unable to find entry");
            return position.value;
        }

        private SkipListNode<TKey, TValue> search(TKey key)
        {
            if (key == null) throw new ArgumentNullException("key");

            SkipListNode<TKey, TValue> current = head;

            while (current.isFront || key.CompareTo(current.key) >= 0)
            {
                if (key.CompareTo(current.key) == 0) return current;

                if (current.forward == null || key.CompareTo(current.forward.key) < 0)
                {
                    if (current.down == null) return current;
                    else current = current.down;
                }
                else
                {
                    current = current.forward;
                }
            }

            return current;
        }

        public void Add(TKey key, TValue value)
        {
            SkipListNode<TKey, TValue> position = search(key);
            if (position != null && position.key.CompareTo(key) == 0)
            {
                position.value = value;
            }
            else
            {
                var newEntry = new SkipListNode<TKey, TValue>((TKey)key, value);
                count++;
                newEntry.back = position;
                if (position.forward != null) newEntry.forward = position.forward;
                position.forward = newEntry;
                promote(newEntry);
            }
        }

        private int levels()
        {
            Random generator = new Random();
            int levels = 0;
            while (generator.NextDouble() < 0.5)
                levels++;
            return levels;
        }

        private void promote(SkipListNode<TKey, TValue> node)
        {
            SkipListNode<TKey, TValue> up = node.back;
            SkipListNode<TKey, TValue> last = node;

            for (int levels = this.levels(); levels > 0; levels--)
            {
                while (up.up == null && !up.isFront)//找到离本次插入的元素左侧最近的有上层元素的节点或者找到head
                    up = up.back;

                if (up.isFront && up.up == null)//如果找到的是head，新建一个空节点，作为新的head
                {
                    up.up = new SkipListNode<TKey, TValue>();
                    head = up.up;
                }

                up = up.up;

                SkipListNode<TKey, TValue> newNode = new SkipListNode<TKey, TValue>(node.keyValue);
                newNode.forward = up.forward;
                up.forward = newNode;

                newNode.down = last;
                last.up = newNode;
                last = newNode;
            }
        }

        public bool Remove(TKey key)
        {
            SkipListNode<TKey, TValue> position = search(key);
            if (position == null || position.isFront || position.key.CompareTo(key) != 0)
            {
                return false;
            }
            else
            {
                SkipListNode<TKey, TValue> old = position;
                do
                {
                    old.back.forward = old.forward;
                    if (old.forward != null) old.forward.back = old.back;
                    old = old.up;
                } while (old != null);
                count--;

                while (head.forward == null)
                {
                    head = head.down;
                }
                return true;
            }
        }
    }
}
