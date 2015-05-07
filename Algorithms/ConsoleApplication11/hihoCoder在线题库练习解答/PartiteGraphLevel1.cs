using System;
using System.Collections.Generic;

namespace Algorithms.hihoCoder在线题库练习解答
{
    /*
     二分图一•二分图判定
     新年回家，又到了一年一度大龄剩男剩女的相亲时间。Nettle去姑姑家玩的时候看到了一张姑姑写的相亲情况表，
     上面都是姑姑介绍相亲的剩男剩女们。每行有2个名字，表示这两个人有一场相亲。由于姑姑年龄比较大了记性不是太好，
     加上相亲的人很多，所以姑姑一时也想不起来其中有些人的性别。因此她拜托我检查一下相亲表里面有没有错误的记录，
     即是否把两个同性安排了相亲。

     对于拿到的相亲情况表，我们不妨将其转化成一个图。将每一个人作为一个点(编号1..N)，若两个人之间有一场相亲，
     则在对应的点之间连接一条无向边。
     因为相亲总是在男女之间进行的，所以每一条边的两边对应的人总是不同性别。假设表示男性的节点染成白色，
     女性的节点染色黑色。对于得到的无向图来说，即每一条边的两端一定是一白一黑。如果存在一条边两端同为白色或者黑色，
     则表示这一条边所表示的记录有误。

     由于我们并不知道每个人的性别，我们的问题就转化为判定是否存在一个合理的染色方案，
     使得我们所建立的无向图满足每一条边两端的顶点颜色都不相同。

     那么，我们不妨将所有的点初始为未染色的状态。随机选择一个点，将其染成白色。再以它为起点，
     将所有相邻的点染成黑色。再以这些黑色的点为起点，将所有与其相邻未染色的点染成白色。不断重复直到整个图都染色完成。
     在染色的过程中，我们应该怎样发现错误的记录呢？相信你一定发现了吧。对于一个已经染色的点，
     如果存在一个与它相邻的已染色点和它的颜色相同，那么就一定存在一条错误的记录。
     */
    /*
     输入
     第1行：1个正整数T(1≤T≤10)
     接下来T组数据，每组数据按照以下格式给出：
     第1行：2个正整数N,M(1≤N≤10,000，1≤M≤40,000)
     第2..M+1行：每行两个整数u,v表示u和v之间有一条边

     输出
     第1..T行：第i行表示第i组数据是否有误。如果是正确的数据输出”Correct”，否则输出”Wrong”

        样例输入
        2
        5 5
        1 2
        1 3
        3 4
        5 2
        1 5
        5 5
        1 2
        1 3
        3 4
        5 2
        3 5
        样例输出
        Wrong
        Correct
     */
    class PartiteGraphLevel1
    {
        class Graph
        {
            private int V = 0;
            private List<int>[] adj = null;

            public Graph(int V)
            {
                this.V = V;
                this.adj = new List<int>[V];
                for (int i = 0; i < V; i++)
                    this.adj[i] = new List<int>();
            }

            public void AddAdj(int i, int j)
            {
                adj[i - 1].Add(j - 1);
                adj[j - 1].Add(i - 1);
            }

            public List<int> GetAdj(int i)
            {
                return adj[i - 1];
            }

            public int GetV()
            {
                return V;
            }
        }

        private bool[] marked = null;
        private int[] colored = null;
        private Graph graph = null;
        private bool result = true;

        public void Start()
        {
            Prepare();
        }

        private void Prepare()
        {
            int count = int.Parse(Console.ReadLine());
            
            for (int i = 0; i < count; i++)
            {
                string V_E = Console.ReadLine();
                string[] VE = V_E.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int V = int.Parse(VE[0]);
                int E = int.Parse(VE[1]);
                graph = new Graph(V);
                for (int j = 0; j < E; j++)
                {
                    string edgeString = Console.ReadLine();
                    string[] edge = edgeString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    int v1 = int.Parse(edge[0]);
                    int v2 = int.Parse(edge[1]);
                    graph.AddAdj(v1, v2);
                }

                Solve(graph);
                if (result) Console.WriteLine("Correct");
                else Console.WriteLine("Wrong");
            }
        }

        private void Solve(Graph graph)
        {
            int V = graph.GetV();
            marked = new bool[V];
            colored = new int[V];
            result = true;

            for (int i = 0; i < V; i++)
                if (!marked[i])
                    DFS(i, 1);
        }

        private void DFS(int current, int color)
        {
            marked[current] = true;
            colored[current] = color;
            var adj = graph.GetAdj(current + 1);
            if (adj == null || adj.Count == 0) return;
            foreach (int next in adj)
            {
                if (!marked[next])
                {
                    DFS(next, -color);
                }
                else
                {
                    if (colored[next] == colored[current] && next != current)
                        result = false;
                }
            }
        }
    }
}
