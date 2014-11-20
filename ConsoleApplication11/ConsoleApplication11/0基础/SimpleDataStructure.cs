namespace ConsoleApplication11
{
    /// <summary>
    /// 数组实现的栈
    /// </summary>
    class ArrayStack<Item> 
    {
        private Item[] a = new Item[1];//初始大小为1
        private int N = 0;
        private void resize(int max)
        {
            Item[] tmp = new Item[max];
            for (int i = 0; i < N; i++) tmp[i] = a[i];
            a = tmp;
        }
        public void push(Item item)
        {
            if (N == a.Length) resize(2 * a.Length);
            a[N] = item;
            N++;
        }
        public Item pop()
        {
            N--;
            Item item = a[N];
            a[N] = default(Item);
            if (N > 0 && N == a.Length / 4) resize(a.Length / 2);
            return item;
        }
    }
    /// <summary>
    /// 链表实现的栈
    /// </summary>
    class LinkedListStack<Item>
    {
        private class Node
        {
            public Item item; 
            public Node next;
            public Node(Item item, Node next)
            {
                this.item = item;
                this.next = next;
            }
        }
        private Node first;
        private int N;
        public void push(Item item)
        {
            Node old = first;
            first = new Node(item, old);
            N++;
        }
        public Item pop()
        {
            if (N == 0) return default(Item);
            Node item = first;
            first = first.next;
            N--;
            return item.item;
        }
    }
    /// <summary>
    /// 链表实现的队列
    /// </summary>
    class LinkedListQueue<Item>
    {
        private class Node
        {
            public Item item;
            public Node next;
            public Node(Item item,Node next)
            {
                this.item = item;
                this.next = next;
            }
        }
        private Node first;
        private Node last;
        private int N;
        public void Enqueue(Item item)
        {
            Node old = last;
            last = new Node(item, null);
            if (N == 0) first = last;
            else old.next = last;
            N++;
        }
        public Item Dequeue()
        {
            if (N == 0) return default(Item);
            Node old = first;
            first = first.next;
            if (first == null) last = null;
            N--;
            return old.item;
        }
    }
}
