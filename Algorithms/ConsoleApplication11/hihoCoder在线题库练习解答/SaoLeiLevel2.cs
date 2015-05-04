using System;

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
                int row = int.Parse(row_column.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                int column = int.Parse(row_column.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);

                int[,] data = new int[row, column];
                int[,] tmp = new int[row, column];
                for (int j = 0; j < row; j++)
                {
                    string rowdata = Console.ReadLine();
                    string[] rowdatas = rowdata.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < column; k++)
                    {
                        data[j, k] = int.Parse(rowdatas[k]);
                        tmp[j, k] = -1;
                    }
                }
                Solve(data, tmp);

                int dileiCnt = 0;
                int notdileiCnt = 0;
                for (int l = 0; l < row; l++)
                {
                    for (int m = 0; m < column; m++)
                    {
                        if (tmp[l, m] == 1) dileiCnt++;
                        else if (tmp[l, m] == 0) notdileiCnt++;
                    }
                }

                Console.WriteLine(dileiCnt + " " + notdileiCnt);
            }
        }

        /// <summary>
        /// 主方法
        /// </summary>
        private void Solve(int[,] data, int[,] tmp)
        {
            int row = data.GetLength(0);
            int column = data.GetLength(1);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (data[i, j] == 0)//运用规则一
                    {
                        find1(data, tmp, i, j);
                    }
                    if (data[i, j] > 0)//运用规则二
                    {
                        find2(data, tmp, i, j);
                        find3(data, tmp, i, j);
                    }
                }
            }
        }

        private void find1(int[,] a, int[,] b, int indexi, int indexj)
        {
            int i, j;
            for (i = -1; i <= 1; i++)
            {
                if (((indexi + i) < 0) || ((indexi + i) >= a.GetLength(0)))
                    continue;
                for (j = -1; j <= 1; j++)
                {
                    if (((indexj + j) < 0) || ((indexj + j) >= a.GetLength(1)) || (i == 0 && j == 0))
                        continue;
                    if (a[indexi + i, indexj + j] == -1)
                    {
                        b[indexi + i, indexj + j] = 0;
                        //Console.WriteLine("find1 " + (indexi + i) + " " + (indexj + j));
                    }
                }
            }
        }

        private void find2(int[,] a, int[,] b, int indexi, int indexj)
        {
            int i, j;
            int count = 0;
            count = AroundUnknown(a, indexi, indexj);
            if (count == a[indexi, indexj])
            {
                for (i = -1; i <= 1; i++)
                {
                    if (((indexi + i) < 0) || ((indexi + i) >= a.GetLength(0)))
                        continue;
                    for (j = -1; j <= 1; j++)
                    {
                        if (((indexj + j) < 0) || ((indexj + j) >= a.GetLength(1)) || (i == 0 && j == 0))
                            continue;
                        if (a[indexi + i, indexj + j] == -1)
                        {
                            b[indexi + i, indexj + j] = 1;
                            //Console.WriteLine("find2 " + (indexi + i) + " " + (indexj + j));
                        }
                    }
                }
            }
        }

        private int AroundUnknown(int[,] map, int indexi, int indexj)
        {
            int i, j;
            int count = 0;
            for (i = -1; i <= 1; i++)
            {
                if (((indexi + i) < 0) || ((indexi + i) >= map.GetLength(0)))
                    continue;
                for (j = -1; j <= 1; j++)
                {
                    if (((indexj + j) < 0) || ((indexj + j) >= map.GetLength(1)) || (i == 0 && j == 0))
                        continue;
                    if (map[indexi + i, indexj + j] == -1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void find3(int[,] map, int[,] b, int indexi, int indexj)
        {
            int i, j;
            int i2, j2;
            int diff;
            for (i = -2; i <= 2; i++)
            {
                if (((indexi + i) < 0) || ((indexi + i) >= map.GetLength(0)))
                    continue;
                for (j = -2; j <= 2; j++)
                {
                    if (((indexj + j) < 0) || ((indexj + j) >= map.GetLength(1)) || (i == 0 && j == 0))
                        continue;
                    if (map[indexi + i, indexj + j] >= 0 && isIn(map, indexi + i, indexj + j, indexi, indexj))
                    {
                        diff = AroundUnknown(map, indexi, indexj) - AroundUnknown(map, indexi + i, indexj + j);
                        if (diff == map[indexi, indexj] - map[indexi + i, indexj + j])
                        {
                            for (i2 = -1; i2 <= 1; i2++)
                            {
                                if (((indexi + i2) < 0) || ((indexi + i2) >= map.GetLength(0)))
                                    continue;
                                for (j2 = -1; j2 <= 1; j2++)
                                {
                                    if (((indexj + j2) < 0) || ((indexj + j2) >= map.GetLength(1)) || (i2 == 0 && j2 == 0))
                                        continue;
                                    if ((map[indexi + i2, indexj + j2] == -1) && (!isAdjacent(indexi + i2, indexj + j2, indexi + i, indexj + j)))
                                    {
                                        b[indexi + i2, indexj + j2] = 1;
                                        Console.WriteLine("find3 " + (indexi + i2) + " " + (indexj + j2));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //判断两点距离,相邻返回1
        private bool isAdjacent(int i1, int j1, int i2, int j2)
        {
            if ((i1 - i2) > 1 || (i1 - i2) < -1 || (j1 - j2) > 1 || (j1 - j2) < -1)
                return false;
            else
                return true;
        }

        //判断点1周围的未知集合是否被点2周围的包含
        private bool isIn(int[,] a, int i1, int j1, int i2, int j2)
        {
            int i, j;
            for (i = -1; i <= 1; i++)
            {
                if (((i1 + i) < 0) || ((i1 + i) >= a.GetLength(0)))
                    continue;
                for (j = -1; j <= 1; j++)
                {
                    if (((j1 + j) < 0) || ((j1 + j) >= a.GetLength(1)) || (i == 0 && j == 0))
                        continue;
                    if (a[i1 + i, j1 + j] == -1 && (!isAdjacent(i1 + i, j1 + j, i2, j2)))
                        return false;
                }
            }
            return true;
        }
    }
}
