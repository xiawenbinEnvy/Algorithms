using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication11._3图
{
    /// <summary>
    /// 加权无向边
    /// </summary>
    public class WeightedEdge : IComparable<WeightedEdge>
    {
        private int v;
        private int w;
        private double weight;

        public WeightedEdge(int _v, int _w, double _weight)
        {
            v = _v;
            w = _w;
            weight = _weight;
        }

        public double GetWeight()
        {
            return weight;
        }

        public int either()
        {
            return v;
        }

        public int other(int thisNode)
        {
            if (thisNode == v) return w;
            if (thisNode == w) return v;
            return -1;
        }

        public int CompareTo(WeightedEdge other)
        {
            if (weight > other.weight) return 1;
            else if (weight < other.weight) return -1;
            else return 0;
        }
    }

    /// <summary>
    /// 加权无向图
    /// </summary>
    public class WeightedEdgeGraph
    {
        private int V;
        private int E;

        private List<WeightedEdge>[] adj;

        public WeightedEdgeGraph(int V)
        {
            this.V = V;
            this.E = 0;
            adj = new List<WeightedEdge>[V];
            for (int i = 0; i < V; i++)
            {
                adj[i] = new List<WeightedEdge>();
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

        public void AddEdge(WeightedEdge e)
        {
            int v = e.either();
            int w = e.other(v);
            adj[v].Add(e);
            adj[w].Add(e);
            E++;
        }

        public List<WeightedEdge> GetAdj(int v)
        {
            return adj[v];
        }

        /// <summary>
        /// 图中所有的边
        /// </summary>
        public List<WeightedEdge> edges()
        {
            List<WeightedEdge> result = new List<WeightedEdge>();
            for (int v = 0; v < V; v++)
            {
                foreach (var e in adj[v])
                {
                    if (e.other(v) > v) result.Add(e);
                }
            }
            return result;
        }
    }

    /// <summary>
    /// 我自己实现的Prim算法,不使用索引优先队列，更好理解
    /// 最小生成树：含有加权无向图的所有顶点的、无环的、连通的、边权值之和最小的子图。
    /// Prim算法不能处理有向图！！！！
    /// </summary>
    class Prim
    {
        Dictionary<int, WeightedEdge> dic = null;
        bool[] marked = null;//已在树中顶点
        Queue<WeightedEdge> queue = null;

        public Prim(WeightedEdgeGraph G)
        {
            dic = new Dictionary<int, WeightedEdge>();
            marked = new bool[G.GetV()];
            queue = new Queue<WeightedEdge>();

            visit(0, G);//从0节点开始
            while (dic.Count > 0)
            {
                int i = GetMinEdge();//最短横切边的非树顶点
                queue.Enqueue(dic[i]);//最短横切边加入queue
                dic.Remove(i);
                visit(i, G);//i被加入树，并且重新计算横切边情况
            }
        }

        /// <summary>
        /// 获取最小生成树的所有边
        /// </summary>
        public IEnumerable<WeightedEdge> GetMST()
        {
            return queue;
        }

        /// <summary>
        /// 找出当前横切边中最短的那条，返回顶点，这个顶点就是下次要被加入树中的那个
        /// </summary>
        private int GetMinEdge()
        {
            double d = double.MaxValue;
            int index = -1;
            foreach (var kv in dic)
            {
                if (kv.Value.GetWeight() < d)
                {
                    d = kv.Value.GetWeight();
                    index = kv.Key;
                }
            }
            return index;
        }

        /// <summary>
        /// v被加入树。
        /// 在图中和v相连但另一个节点是非树中的所有边中
        /// 找出最短的那条，入字典，以供下次取最短横切边时使用
        /// </summary>
        /// <param name="v">要被加入树中的节点</param>
        private void visit(int v, WeightedEdgeGraph G)
        {
            marked[v] = true;//v被加入树
            foreach (var e in G.GetAdj(v))
            {
                int w = e.other(v);
                if (marked[w]) continue;

                if (dic.ContainsKey(w))//w节点还连接到别的树节点上
                {
                    if (dic[w].GetWeight() > e.GetWeight()) //此时e权重更短的话，进行替换 
                        dic[w] = e;
                }
                else//w节点还没连接到别的树节点上
                {
                    dic.Add(w, e);
                }
            }
        }
    }

    /// <summary>
    /// 按照权重由小到大并且与树中已有边不会形成环的次序来加入最小生成树，直到树中边的数目为（顶点数-1）
    /// </summary>
    class Kruskal
    {
        Queue<WeightedEdge> queue = null;
        PQ<WeightedEdge> pq = null;
        int[] union = null;
        public Kruskal(WeightedEdgeGraph G)
        {
            queue = new Queue<WeightedEdge>();
            pq = new MinPQ<WeightedEdge>(G.GetE());
            union = new int[G.GetV()];
            for (int x = 0; x < union.Count(); x++)//初始化连通分量
                union[x] = x;
            foreach (var e in G.edges())//初始化优先队列
                pq.insert(e);

            while (!pq.IsEmpty() && queue.Count < G.GetV() - 1)
            {
                var minE = pq.DeletePQHead();//最短边
                int v = minE.either();
                int w = minE.other(v);
                if (isConnect(v, w)) continue;//会形成环

                queue.Enqueue(minE);
                connect(v, w);//加入同一连通分量
            }
        }

        /// <summary>
        /// 判断是否会形成环
        /// </summary>
        private bool isConnect(int v, int w)
        {
            return find(v) == find(w);
        }
        /// <summary>
        /// 找到节点所属连通分量
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int find(int i)
        {
            while (i != union[i])
                i = union[i];

            return i;
        }
        /// <summary>
        /// 将两个节点分入同一连通分量
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        private void connect(int v, int w)
        {
            int i = find(v);
            int j = find(w);
            if (i == j) return;
            union[i] = j;
        }

        /// <summary>
        /// 获取最小生成树的所有边
        /// </summary>
        public IEnumerable<WeightedEdge> GetMST()
        {
            return queue;
        }
    }
}
