using System;
using System.Collections.Generic;

namespace ConsoleApplication11
{
    /// <summary>
    /// 最短增广路径的FordFulkerson最大流量算法
    /// </summary>
    class FordFulkerson
    {
        private bool[] marked;
        private FlowEdge[] edgeTo;
        private double value;//最大流量

        public FordFulkerson(FlowNetwork G, int s, int t)
        {
            while (hasAugmentingPath(G, s, t))
            {
                //Stack<int> stack = new Stack<int>();
                
                double bottle = double.MaxValue;
                for (int v = t; v != s; v = edgeTo[v].other(v))
                    bottle = Math.Min(bottle, edgeTo[v].residualCapacityTo(v));//该条增广路径的最大流量(取决于容量最小的那条边)
                for (int v = t; v != s; v = edgeTo[v].other(v))
                    edgeTo[v].addReaidualFlowTo(v, bottle);//改变增广路径中的每条边的剩余容量
                //for (int v = t; v != s; v = edgeTo[v].other(v))
                   // stack.Push(v);
                value += bottle;
                //while (stack.Count > 0)
                //{
                //    int pop = stack.Pop();
                //    if (pop != 0 && pop != 13)
                //        Console.WriteLine(pop);
                //}
            }
        }

        public double Value()
        {
            return value;
        }

        /// <summary>
        /// 在剩余网络中，利用BFS寻找增广路径
        /// </summary>
        private bool hasAugmentingPath(FlowNetwork G, int s, int t)
        {
            marked = new bool[G.v()];
            edgeTo = new FlowEdge[G.v()];

            Queue<int> q = new Queue<int>();
            marked[s] = true;
            q.Enqueue(s);

            while (q.Count > 0)
            {
                int v = q.Dequeue();
                foreach (var edge in G.adj(v))
                {
                    int w = edge.other(v);

                    if (!marked[w] && edge.residualCapacityTo(w) > 0)
                    {
                        edgeTo[w] = edge;
                        marked[w] = true;
                        q.Enqueue(w);
                    }
                }
            }

            return marked[t];
        }
    }

    /// <summary>
    /// 流量网络中的边
    /// </summary>
    class FlowEdge
    {
        private int v;//起点
        private int w;//终点

        private double c;//容量
        private double f;//流量

        public FlowEdge(int v, int w, double c)
        {
            this.v = v;
            this.w = w;
            this.c = c;
            this.f = 0.0;
        }

        /// <summary>
        /// 求vertex的剩余容量
        /// </summary>
        public double residualCapacityTo(int vertex)
        {
            if (vertex == v) return f;
            else if (vertex == w) return c - f;
            else throw new Exception("不可能");
        }

        /// <summary>
        /// 将vertex的流量增加delta
        /// </summary>
        public void addReaidualFlowTo(int vertex, double delta)
        {
            if (vertex == v) f -= delta;
            else if (vertex == w) f += delta;
        }

        public int from()
        {
            return v;
        }

        public int to()
        {
            return w;
        }

        public int other(int o)
        {
            if (o == v) return w;
            else if (o == w) return v;
            else throw new Exception("不可能");
        }
    }

    /// <summary>
    /// 流量网络图
    /// </summary>
    class FlowNetwork
    {
        private int V;//顶点数
        private int E;//边数
        private List<FlowEdge>[] _adj;//邻接表

        public FlowNetwork(int V)
        {
            this.V = V;
            this.E = 0;
            _adj = new List<FlowEdge>[V];
            for (int v = 0; v < V; v++)
                _adj[v] = new List<FlowEdge>();
        }

        public void addEdge(FlowEdge e)
        {
            int v = e.from();
            int w = e.to();
            _adj[v].Add(e);//一开始就将原本的有向图变成无向图，避免了方向的反转，很巧妙！！
            _adj[w].Add(e);
            E++;
        }

        public List<FlowEdge> adj(int v)
        {
            return _adj[v];
        }

        public int v()
        {
            return V;
        }
    }
}
