using System.Collections.Generic;
using ConsoleApplication11._3图;

namespace ConsoleApplication11
{
    /// <summary>
    /// 只支持连接、或（可以多个）、闭包、括号、通配符的正则表达式模式匹配
    /// (不支持处理元字符、字符集描述符、闭包缩略写法)
    /// 约定所有模式都包含在括号中
    /// </summary>
    class NFA//((A*B|AC)D)测试用例
    {
        private char[] re;//匹配转换
        private DirectGraph G;//epsilon转换
        private int M;//状态数量,即正则表达式长度

        /// <summary>
        /// 根据给定的正则表达式构造NFA
        /// </summary>
        public NFA(string regexp)
        {
            Stack<int> ops = new Stack<int>();//用来保存左括号、或符号
            re = regexp.ToCharArray();
            M = re.Length;
            G = new DirectGraph(M + 1);//有向图，保存每个点出发可以到达的点

            for (int i = 0; i < M; i++)
            {
                int lp = i;
                if (re[i] == '(' || re[i] == '|')//|的索引也要压入栈中，这样因为（的索引也压入了，可以遇到右括号时，（和|的索引都能知道，就能实现或跳转了
                {
                    ops.Push(i);
                }
                else if (re[i] == ')')
                {
                    List<int> orList = new List<int>();
                    while (ops.Count > 0)
                    {
                        int or = ops.Pop();
                        if (re[or] == '(')//找到|符号的上一个左括号的位置
                        {
                            lp = or;
                            break;
                        }
                        if (re[or] == '|') orList.Add(or);
                    }
                    foreach (var or in orList)
                    {
                        G.AddAdj(lp, or + 1);//把（和|后面的第一个字符相连
                        G.AddAdj(or, i);//把|和)相连
                    }
                }
                if (i < M - 1 && re[i + 1] == '*')//当前字符下一字符为闭包
                {
                    G.AddAdj(lp, i + 1);//即B->*
                    G.AddAdj(i + 1, lp);//即*->B
                }
                if (re[i] == '(' || re[i] == '*' || re[i] == ')')//这些符号也都可以进入下一状态，但|不能直接进入下一状态。普通字符之间不需要进行连接，因为不是epsilon转换
                {
                    G.AddAdj(i, i + 1);
                }
            }
        }
        /// <summary>
        /// 模拟NFA在给定文本上的运行轨迹
        /// </summary>
        public bool recognizes(string txt)
        {
            List<int> pc = new List<int>();//表示从reg某一位可达的后续位置

            GraphDFS dfs = new GraphDFS(G, 0);//先计算从0开始可达的所有状态
            for (int v = 0; v < G.GetV(); v++)
                if (dfs.hasPathTo(v))
                    pc.Add(v);

            for (int i = 0; i < txt.Length; i++)
            {
                List<int> match = new List<int>();//match表示txt字符在reg中匹配后，下一位txt字符去匹配的reg位置
                foreach (int v in pc)
                    if (v < M)
                        if (re[v] == txt[i] || re[v] == '.')
                            match.Add(v + 1);//因为是给下一匹配用的，所以要加1

                pc = new List<int>();
                dfs = new GraphDFS(G, match);
                for (int v = 0; v < G.GetV(); v++)//重新计算目前reg所在位置的后续所有可达状态
                    if (dfs.hasPathTo(v))
                        pc.Add(v);
            }

            foreach (int v in pc)
                if (v == M)
                    return true;//到达了正则的最后一位，表示匹配

            return false;//不匹配
        }
    }
}
