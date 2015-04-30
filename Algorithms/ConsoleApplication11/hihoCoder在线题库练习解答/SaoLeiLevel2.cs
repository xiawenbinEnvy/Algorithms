using System;
using System.Collections.Generic;

namespace Algorithms.hihoCoder在线题库练习解答
{
    /*
     给出一张N*M的地图，其中的某些格子已经被探明，探明的格子中都标有一个数字，表明与它相邻的8个格子里的地雷数量，
     而没有探明的格子里可能会有一些地雷。
     现在小Ho仅仅知道如下三条规则：
     1.如果某一个被探明的格子里所标的数字为0，那么它相邻的8个格子里的未探明格子被认作是一定不是地雷的格子。
     2.如果某一个被探明的格子里所标的数字为K，且它相邻的8个格子里正好有K个没有探明的格子的话，
     则这K个没有探明的格子被认作是一定是地雷的格子。
     3.如果某两个探明了的格子A和B，他们中标明的数字分别为P_A和P_B，且他们周围8个格子中没有探明的格子组成的集合分别为S_A和S_B，
     如果S_A包含S_B，且|S_A|-|S_B|=P_A-P_B，那么S_A-S_B中的所有格子，被认作是一定是地雷的格子。
     而问题是：仅仅利用这三条规则，小Ho到底能判断出哪些没有探明的格子里一定是地雷，而哪些没有探明的格子里一定不是地雷呢？
      
     输入
     每个测试点（输入文件）存在多组测试数据。
     每个测试点的第一行为一个整数Task，表示测试数据的组数。
     在一组测试数据中：
     第1行为2个整数N，M，表示迷宫的大小。
     接下来的N行，每行M个整数，为一个矩阵A，用以描述整个迷宫，其中对于每一个格子A[i][j]，
     若A[i][j]=-1，则表示该格子没有被探明，若0<=A[i][j]<=8，则表示该格子已经被探明了，且数值代表该格子附近8个格子中的地雷数。<>
     对于100%的数据，满足：5<=N、M<=2*10^2, -1<=A[i][j]<=8。<>
     对于100%的数据，满足：符合数据描述的迷宫一定存在。

     输出
     对于每组测试数据，输出2个整数，分别为一定为地雷的未探明格子数和一定不为地雷的未探明格子数。
     
     样例输入
     2
     7 10
     0  0 -1  1 -1  1 -1  0 -1  0
    -1 -1  0  1 -1 -1  0 -1  0 -1
    -1 -1 -1 -1 -1  1  0  0  0  0
     1 -1  0  0 -1  0 -1 -1 -1  0
    -1 -1  0 -1 -1 -1 -1  0 -1 -1
     0  0  0  0  0 -1 -1  1 -1  0
    -1 -1  0  0 -1 -1  1 -1  1  0
     8 8
     -1 -1 -1  2  3  2  1 -1
      2  -1 -1 -1 -1 -1  1  0
     -1 -1  1 -1  3 -1 -1 -1
     -1 -1 -1 -1 -1 -1 -1  1
     -1  2 -1 -1 -1 -1 -1 -1
      0 -1 -1  2 -1  2 -1  2
      1 -1  3 -1  1  1 -1 -1
     -1 -1  2  1  1 -1 -1 -1
     样例输出
     1 32
     4 6
     */
    class SaoLeiLevel2
    {
        private const int Unknown = -1;
        private const int NotDiLei = -100;
        private const int DiLei = -999;

        public void Start() 
        {
            Prepare();
        }

        /// <summary>
        /// 从控制台输入
        /// </summary>
        private void Prepare()
        {
            int groupCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < groupCount; i++)
            {
                string row_column = Console.ReadLine();
                int row = int.Parse(row_column.Split(' ')[0]);
                int column = int.Parse(row_column.Split(' ')[1]);

                int[,] data = new int[row, column];
                for (int j = 0; j < row; j++)
                {
                    string rowdata = Console.ReadLine();
                    string[] rowdatas = rowdata.Split(' ');

                    for (int k = 0; k < column; k++)
                    {
                        data[j, k] = int.Parse(rowdatas[k]);
                    }
                }
                Solve(data);

                int dileiCnt = 0;
                int notdileiCnt = 0;
                for (int l = 0; l < data.GetLength(0); l++)
                {
                    for (int m = 0; m < data.GetLength(1); m++)
                    {
                        if (data[l, m] == DiLei) dileiCnt++;
                        else if (data[l, m] == NotDiLei) notdileiCnt++;
                    }
                }

                Console.WriteLine(dileiCnt + " " + notdileiCnt);
            }
        }

        /// <summary>
        /// 主方法
        /// </summary>
        private void Solve(int[,] data)
        {
            int row = data.GetLength(0);
            int column = data.GetLength(1);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (data[i, j] == Unknown)
                    {
                        continue;
                    }

                    if (data[i, j] == 0)//运用规则一
                    {
                        SetUnknownRule1_2(data, i, j, NotDiLei);
                    }
                    else if (data[i, j] > 0)//运用规则二
                    {
                        int unknownCnt = AroundUnknown(data, i, j).Count;
                        if (unknownCnt == data[i, j])
                        {
                            SetUnknownRule1_2(data, i, j, DiLei);
                        }
                        else//运用规则三
                        {
                            SetUnknownRule3(data, i, j);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 规则一、二
        /// </summary>
        private void SetUnknownRule1_2(int[,] map, int i, int j, int value)
        {
            for (int ii = -1; ii <= 1; ii++)//-1~1的范围意思是左右、上下各有一个空间
            {
                for (int jj = -1; jj <= 1; jj++)
                {
                    if (ii == 0 && jj == 0) continue;

                    int _i = i + ii;
                    int _j = j + jj;

                    if (_i >= 0 && _i < map.GetLength(0) && _j >= 0 && _j < map.GetLength(1) && map[_i, _j] == Unknown)
                    {
                        map[_i, _j] = value;
                    }
                }
            }
        }

        /// <summary>
        /// 获取周围8个格子内所有未探明的格子的坐标
        /// </summary>
        private List<Tuple<int, int>> AroundUnknown(int[,] map, int i, int j)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int ii = -1; ii <= 1; ii++)
            {
                for (int jj = -1; jj <= 1; jj++)
                {
                    if (ii == 0 && jj == 0) continue;

                    int _i = i + ii;
                    int _j = j + jj;

                    if (_i >= 0 && _i < map.GetLength(0) && _j >= 0 && _j < map.GetLength(1) && map[_i, _j] == Unknown)
                    {
                        result.Add(Tuple.Create(_i, _j));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取能有交集的范围内的格子内所有标记地雷数大于0的格子的坐标
        /// </summary>
        private List<Tuple<int, int>> AroundHasDiLei(int[,] map, int i, int j)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int ii = -2; ii <= 2; ii++)//上下左右差2，才会有包含关系
            {
                for (int jj = -2; jj <= 2; jj++)
                {
                    if (ii == 0 && jj == 0) continue;

                    int _i = i + ii;
                    int _j = j + jj;

                    if (_i >= 0 && _i < map.GetLength(0) && _j >= 0 && _j < map.GetLength(1) && map[_i, _j] > 0)
                    {
                        result.Add(Tuple.Create(_i, _j));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 规则三
        /// </summary>
        private void SetUnknownRule3(int[,] map, int i, int j)
        {
            var thisAroundUnknown = AroundUnknown(map, i, j);//S_A
            if (thisAroundUnknown.Count == 0) return;

            var aroundHasDiLei = AroundHasDiLei(map, i, j);//先找到周围2个单元格范围内，地雷数大于0的格子的坐标
            foreach (var position in aroundHasDiLei)
            {
                if (map[i, j] <= map[position.Item1, position.Item2]) continue;

                var otherAroundUnknown = AroundUnknown(map, position.Item1, position.Item2);//S_B
                if (thisAroundUnknown.Count <= otherAroundUnknown.Count) continue;

                var contains = GetContain(thisAroundUnknown, otherAroundUnknown);
                if (contains == null || contains.Count == 0) continue;
                if (contains.Count == map[i, j] - map[position.Item1, position.Item2])
                {
                    foreach (var t in contains)
                    {
                        SetUnknownRule1_2(map, t.Item1, t.Item2, DiLei);
                    }
                }
            }
        }

        /// <summary>
        /// 判断两个集合的交集关系，并且返回非公共的元素
        /// </summary>
        private List<Tuple<int, int>> GetContain(List<Tuple<int, int>> _this, List<Tuple<int, int>> _other)
        {
            List<Tuple<int, int>> contains = new List<Tuple<int, int>>();
            int count = 0;
            foreach (var a in _this)
            {
                bool found = false;
                foreach (var b in _other)
                {
                    if (a == b)
                    {
                        found = true;
                        count++;
                        break;
                    }
                }
                if (!found)
                    contains.Add(a);//就是多出来的那个
            }

            if (count != _other.Count) contains = null;//不是交集关系
            return contains;
        }
    }
}
