using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication11._3图
{
    /// <summary>
    /// 加权有向边
    /// </summary>
    class DirectedWeightedEdge : IComparable<DirectedWeightedEdge>
    {
        private int to;
        private int from;
        private double weight;

        public DirectedWeightedEdge(int _from, int _to, double _weight)
        {
            from = _from;
            to = _to;
            weight = _weight;
        }

        public double GetWeight()
        {
            return weight;
        }

        public int From()
        {
            return from;
        }

        public int To()
        {
            return to;
        }

        public int CompareTo(DirectedWeightedEdge other)
        {
            if (weight > other.weight) return 1;
            else if (weight < other.weight) return -1;
            else return 0;
        }
    }

    /// <summary>
    /// 加权有向图
    /// </summary>
    class DirectedWeightedEdgeGraph
    {
        private int V;
        private int E;

        private List<DirectedWeightedEdge>[] adj;

        public DirectedWeightedEdgeGraph(int V)
        {
            this.V = V;
            this.E = 0;
            adj = new List<DirectedWeightedEdge>[V];
            for (int i = 0; i < V; i++)
            {
                adj[i] = new List<DirectedWeightedEdge>();
            }
        }

        public int GetV()
        {
            return V;
        }

        public int GetE()
        {
            return E;
        }

        public void AddEdge(DirectedWeightedEdge e)
        {
            E++;
            adj[e.From()].Add(e);
        }

        public List<DirectedWeightedEdge> GetAdj(int v)
        {
            return adj[v];
        }
    }

    /// <summary>
    /// 最短路径算法，我自己实现的，不用索引优先队列(适用于 正权重、无论是否有环、无论是否有向 的图)
    /// 注意：边的权重必须为正！！！
    /// </summary>
    class Dijkstra
    {
        private double[] distTo = null;
        private DirectedWeightedEdge[] edgeTo = null;
        private Dictionary<int, double> dic = null;
        public Dijkstra(DirectedWeightedEdgeGraph G, int s)
        {
            distTo = new double[G.GetV()];
            edgeTo = new DirectedWeightedEdge[G.GetE()];
            dic = new Dictionary<int, double>();

            for (int i = 0; i < distTo.Length; i++)//到所有顶点的距离初始化为最大值
                distTo[i] = double.MaxValue;

            distTo[s] = 0;//从0节点开始
            dic.Add(0, 0);
            while (dic.Count() > 0)//直到所有节点都处理完为止
            {
                var minE = MinEdge();//取出目前最短的路径的终点
                dic.Remove(minE);
                relax(minE, G);//开始放松
            }
        }

        private int MinEdge()
        {
            double d = double.MaxValue;
            int i = -1;
            foreach (var kv in dic)
            {
                if (kv.Value < d)
                {
                    d = kv.Value;
                    i = kv.Key;
                }
            }
            return i;
        }

        private void relax(int v, DirectedWeightedEdgeGraph G)
        {
            foreach (var e in G.GetAdj(v))
            {
                int w = e.To();
                if (distTo[w] > distTo[v] + e.GetWeight())//到w的已有距离大于新节点到他的距离+新节点已有的距离
                {
                    distTo[w] = distTo[v] + e.GetWeight();//放松操作
                    edgeTo[w] = e;
                    if (dic.ContainsKey(w)) dic[w] = distTo[w];//保存下来
                    else dic.Add(w, distTo[w]);
                }
            }
        }

        public double DistTo(int w)
        {
            return distTo[w];
        }

        public bool hasPathTo(int w)
        {
            return distTo[w] < double.MaxValue;
        }

        public IEnumerable<DirectedWeightedEdge> PathTo(int w)
        {
            if (!hasPathTo(w)) throw new Exception();
            Stack<DirectedWeightedEdge> result = new Stack<DirectedWeightedEdge>();
            for (var e = edgeTo[w]; e != null; e = edgeTo[e.From()])
                result.Push(e);
            return result;
        }
    }

    /// <summary>
    /// 最短路径算法2（适用于 无环有向、无论正负权 的图）——无环有向的限制来源于拓扑排序
    /// 第零步：从加权图造出一个无权图
    /// 第一步：用无权图求出图的拓扑排序
    /// 第二步：用拓扑排序的顺序对加权图进行relax
    /// 
    /// 注意：只适用于无环的加权有向图
    /// </summary>
    class AcyclicSP
    {
        protected DirectedWeightedEdge[] edgeTo = null;//也就是它的最短路径树
        protected double[] distTo = null;
        private DirectGraph tmpG = null;

        public AcyclicSP(DirectedWeightedEdgeGraph G, int s)
        {
            //从加权有向图中获取它的有向图，以供拓扑排序用
            tmpG = new DirectGraph(G.GetV());
            for (int v = 0; v < G.GetV(); v++)
            {
                foreach (var e in G.GetAdj(v))
                {
                    int to = e.To();
                    tmpG.AddAdj(v, to);
                }
            }

            int V = G.GetV();
            edgeTo = new DirectedWeightedEdge[V];
            distTo = new double[V];
            for (int i = 0; i < V; i++)
            {
                distTo[i] = double.MaxValue;
            }

            distTo[s] = 0.0;
            Topological top = new Topological(tmpG);
            foreach (var v in top.Order)
            {
                relax(G, v);
            }
        }

        protected void relax(DirectedWeightedEdgeGraph G, int v)
        {
            foreach (var e in G.GetAdj(v))
            {
                int w = e.To();
                if (distTo[w] > e.GetWeight() + distTo[v])
                {
                    edgeTo[w] = e;
                    distTo[w] = e.GetWeight() + distTo[v];
                }
            }
        }

        public double DistTo(int v)
        {
            return distTo[v];
        }

        public bool hasPathTo(int v)
        {
            return distTo[v] < double.MaxValue;
        }

        public IEnumerable<DirectedWeightedEdge> pathTo(int v)
        {
            if (!hasPathTo(v)) return null;

            Stack<DirectedWeightedEdge> result = new Stack<DirectedWeightedEdge>();
            for (var i = edgeTo[v]; i != null; i = edgeTo[i.From()])
            {
                result.Push(i);
            }
            return result;
        }
    }
}
