using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication11._3图
{
    /// <summary>
    /// 可以有 无论正负权重边、可以有环，但从s到t的最短路径上不允许出现负权重环 的最短路径算法
    /// 注意：不能有负权重环！！！
    /// </summary>
    class BellmanFord
    {
        double[] disTo = null;
        DirectedWeightedEdge[] edges = null;
        bool[] inQueue = null;
        Queue<int> queue = null;
        int cost;
        IEnumerable<DirectedWeightedEdge> cycle = null;
        public BellmanFord(DirectedWeightedEdgeGraph G, int s)
        {
            disTo = new double[G.GetV()];
            edges = new DirectedWeightedEdge[G.GetE()];
            inQueue = new bool[G.GetV()];
            queue = new Queue<int>();

            for (int i = 0; i < G.GetV(); i++)
                disTo[i] = double.MaxValue;

            disTo[s] = 0;
            inQueue[s] = true;
            queue.Enqueue(s);

            while (queue.Count() > 0 && !hasNegativeCycle())//没有负权重环出现
            {
                int v = queue.Dequeue();
                inQueue[v] = false;
                relax(v, G);
            }
        }

        private bool hasNegativeCycle()
        {
            return cycle != null;
        }
        private void relax(int v, DirectedWeightedEdgeGraph G)
        {
            foreach (var e in G.GetAdj(v))
            {
                int w = e.To();
                if (disTo[w] > disTo[v] + e.GetWeight())
                {
                    disTo[w] = disTo[v] + e.GetWeight();
                    edges[w] = e;

                    if (!inQueue[w])
                    {
                        inQueue[w] = true;
                        queue.Enqueue(w);
                    }
                }

                if (cost++ % G.GetV() == 0)
                    findNegativeCycle();
            }
        }

        /// <summary>
        /// 用edges[]中的边来构造一幅加权有向图来检测环——基本思路：
        /// 在图中dfs，判断到目前节点已被marked并且在栈中，就是一个环
        /// </summary>
        private void findNegativeCycle()
        {
            int V = edges.Length;
            DirectedWeightedEdgeGraph G = new DirectedWeightedEdgeGraph(V);
            foreach (var e in edges)//用edges来构造一幅加权有向图
                if (e != null)
                    G.AddEdge(e);

            DirectedWeightedCycle dc = new DirectedWeightedCycle(G);
            cycle = dc.GetCycle();//疑问：貌似找到的不一定是负权重环？？？？
        }

        class DirectedWeightedCycle
        {
            bool[] marked = null;
            bool[] onstack = null;
            DirectedWeightedEdge[] edgeTo = null;
            Stack<DirectedWeightedEdge> cycle = null;
            public DirectedWeightedCycle(DirectedWeightedEdgeGraph G)
            {
                marked = new bool[G.GetV()];
                onstack = new bool[G.GetV()];
                edgeTo = new DirectedWeightedEdge[G.GetE()];

                for (int v = 0; v < G.GetV(); v++)
                    if (!marked[v]) dfs(v, G);
            }

            private void dfs(int v, DirectedWeightedEdgeGraph G)
            {
                marked[v] = true;
                onstack[v] = true;
                foreach (var e in G.GetAdj(v))
                {
                    var w = e.To();
                    if (cycle != null)
                    {
                        return;
                    }
                    else if (!marked[w])
                    {
                        edgeTo[w] = e;
                        dfs(w, G);
                    }
                    else if (onstack[w])
                    {
                        cycle = new Stack<DirectedWeightedEdge>();
                        for (var ee = e; ee.From() != w; ee = edgeTo[e.From()])
                            cycle.Push(e);
                        cycle.Push(e);
                    }
                }
                onstack[v] = false;
            }

            public IEnumerable<DirectedWeightedEdge> GetCycle()
            {
                return cycle;//貌似找到的环不一定是负权重的？？？？？
            }
        }
    }
}
