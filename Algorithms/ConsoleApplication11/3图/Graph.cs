using System.Collections.Generic;

namespace ConsoleApplication11._3图
{
    /// <summary>
    /// 图定义
    /// </summary>
    public abstract class Graph
    {
        /// <summary>
        /// 节点数
        /// </summary>
        protected int V;
        /// <summary>
        /// 边数
        /// </summary>
        protected int E;
        /// <summary>
        /// 节点数组，索引为节点编号，值为此节点的所有相邻节点(邻接表数组)
        /// </summary>
        protected List<int>[] adj = null;

        public Graph(int V)
        {
            adj = new List<int>[V];
            for (int i = 0; i < V; i++)
            {
                adj[i] = new List<int>();
            }
            this.V = V;
            this.E = 0;
        }

        /// <summary>
        /// 添加一组可达节点
        /// </summary>
        public abstract void AddAdj(int s, int e);

        /// <summary>
        /// 返回节点的所有相邻节点
        /// </summary>
        public List<int> GetAdj(int n)
        {
            return adj[n];
        }

        /// <summary>
        /// 获取图的节点数
        /// </summary>
        public int GetV()
        {
            return V;
        }
        /// <summary>
        /// 获取图的边的数
        /// </summary>
        public int GetE()
        {
            return E;
        }
    }
    /// <summary>
    /// 无向图
    /// </summary>
    class NoDirectGraph : Graph
    {
        public NoDirectGraph(int V)
            : base(V)
        { }
        public override void AddAdj(int s, int e)
        {
            adj[s].Add(e);
            adj[e].Add(s);
            E++;
        }
    }
    /// <summary>
    /// 有向图
    /// </summary>
    class DirectGraph : Graph
    {
        public DirectGraph(int V)
            : base(V)
        { }
        public override void AddAdj(int s, int e)
        {
            adj[s].Add(e);
            E++;
        }
    }

    /// <summary>
    /// 图的搜索
    /// </summary>
    abstract class GraphSearch
    {
        /// <summary>
        /// 图
        /// </summary>
        protected Graph G = null;
        /// <summary>
        /// 起点
        /// </summary>
        protected int start = 0;

        /// <summary>
        /// 节点是否已被访问
        /// </summary>
        protected bool[] marked = null;
        /// <summary>
        /// 到达节点的轨迹
        /// </summary>
        protected int[] edgeTo = null;

        public GraphSearch(Graph _G, int _start)
        {
            G = _G;
            start = _start;

            marked = new bool[G.GetV()];
            edgeTo = new int[G.GetV()];

            Search(start);
        }

        public GraphSearch(Graph _G)
        {
            G = _G;

            marked = new bool[G.GetV()];
            edgeTo = new int[G.GetV()];
        }

        public GraphSearch(Graph _G, List<int> sources)
        {
            G = _G;
            marked = new bool[G.GetV()];
            edgeTo = new int[G.GetV()];

            foreach (int s in sources)
                if (!marked[s])
                    Search(s);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        protected abstract void Search(int n);

        /// <summary>
        /// 判断n和起点是否联通
        /// </summary>
        public bool hasPathTo(int n)
        {
            return marked[n];
        }

        /// <summary>
        /// 返回从起点到达n的轨迹
        /// </summary>
        public IEnumerable<int> GetPath(int n)//回溯
        {
            if (!hasPathTo(n)) return null;

            Stack<int> stack = new Stack<int>();
            for (int i = n; i != start; i = edgeTo[i])
            {
                stack.Push(i);
            }
            stack.Push(start);

            return stack;
        }
    }
    /// <summary>
    /// 深度优先(不能得到单点最短路径，通过使用递归来实现)
    /// </summary>
    class GraphDFS : GraphSearch
    {
        public GraphDFS(Graph _G, int _start)
            : base(_G, _start)
        {
        }

        public GraphDFS(Graph _G, List<int> _source)
            : base(_G, _source)
        {
        }

        protected override void Search(int n)
        {
            marked[n] = true;
            foreach (var i in G.GetAdj(n))
            {
                if (!marked[i])
                {
                    edgeTo[i] = n;

                    Search(i);
                }
            }
        }
    }
    /// <summary>
    /// 广度优先(能得到单点最短路径，通过使用队列【FIFO】来实现)
    /// </summary>
    class GraphBFS : GraphSearch
    {
        public GraphBFS(Graph _G, int _start)
            : base(_G, _start)
        {

        }

        protected override void Search(int n)
        {
            Queue<int> queue = new Queue<int>();
            marked[n] = true;
            queue.Enqueue(n);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();

                List<int> l = G.GetAdj(v);

                foreach (var i in l)
                {
                    if (!marked[i])
                    {
                        marked[i] = true;
                        edgeTo[i] = v;

                        queue.Enqueue(i);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 用深度优先来判断图是二分图吗（双色问题）
    /// </summary>
    class TwoColor : GraphSearch
    {
        private bool[] color;
        private bool isTwoColorable = true;
        public bool IsTwoColorable { get { return isTwoColorable; } }

        public TwoColor(Graph G)
            : base(G)
        {
            color = new bool[G.GetV()];
            for (int s = 0; s < G.GetV(); s++)
            {
                if (!marked[s])
                {
                    Search(s);
                }
            }
        }

        protected override void Search(int n)
        {
            marked[n] = true;
            foreach (var w in G.GetAdj(n))
            {
                if (!marked[w])
                {
                    color[w] = !color[n];
                    Search(w);
                }
                else
                {
                    if (color[w] == color[n])
                        isTwoColorable = false;
                }
            }
        }
    }
    /// <summary>
    /// 用深度优先在有向图中寻找有向环
    /// </summary>
    class DirectCycle : GraphSearch
    {
        private bool[] onStack = null;

        private Stack<int> cycle = null;

        public DirectCycle(DirectGraph _G)
            : base(_G)
        {
            onStack = new bool[G.GetV()];
            for (int t = 0; t < G.GetV(); t++)
            {
                if (!marked[t])
                {
                    Search(t);
                }
            }
        }
        protected override void Search(int n)
        {
            marked[n] = true;
            onStack[n] = true;
            foreach (var i in G.GetAdj(n))
            {
                if (!marked[i])
                {
                    edgeTo[i] = n;
                    Search(i);
                }
                else if (onStack[i])//找到了环
                {
                    cycle = new Stack<int>();
                    for (int x = n; x != i; x = edgeTo[x])
                    {
                        cycle.Push(x);
                    }
                    cycle.Push(i);
                    cycle.Push(n);
                    return;
                }
            }
            onStack[n] = false;
        }

        public bool hasCycle()
        {
            return cycle != null;
        }

        public IEnumerable<int> GetCycle()
        {
            return cycle;
        }
    }
    /// <summary>
    /// 用深度优先获取有向图所有顶点的逆后序
    /// </summary>
    class DFSOrder : GraphSearch
    {
        private Stack<int> reversePost;
        /// <summary>
        /// 顶点的逆后序排列
        /// </summary>
        public IEnumerable<int> ReversePost { get { return reversePost; } }

        public DFSOrder(DirectGraph G)
            : base(G)
        {
            reversePost = new Stack<int>();
            for (int v = 0; v < G.GetV(); v++)
            {
                if (!marked[v])
                {
                    Search(v);
                }
            }
        }

        protected override void Search(int n)
        {
            marked[n] = true;
            foreach (var w in G.GetAdj(n))
            {
                if (!marked[w])
                {
                    Search(w);
                }
            }
            reversePost.Push(n);
        }
    }
    /// <summary>
    /// 有向图的拓扑排序（无环有向图的所有顶点的逆后序）
    /// </summary>
    class Topological
    {
        private IEnumerable<int> order = null;
        public IEnumerable<int> Order { get { return order; } }
        public Topological(DirectGraph G)
        {
            DirectCycle cycle = new DirectCycle(G);
            if (!cycle.hasCycle())
            {
                DFSOrder dfsOrder = new DFSOrder(G);
                order = dfsOrder.ReversePost;
            }
        }
        public bool CanTopological()
        {
            return order != null;
        }
    }

    /// <summary>
    /// 图的可达性——图中任意给定的两点是否可达
    /// </summary>
    class TranstiveClosure
    {
        private GraphDFS[] all;
        public TranstiveClosure(Graph G)
        {
            all = new GraphDFS[G.GetV()];
            for (int i = 0; i < G.GetV(); i++)
                all[i] = new GraphDFS(G, i);
        }

        public bool reachable(int v, int w)
        {
            return all[v].hasPathTo(w);
        }
    }
}
