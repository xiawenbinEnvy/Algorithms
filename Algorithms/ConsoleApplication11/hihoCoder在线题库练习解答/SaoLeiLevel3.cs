using System;

namespace Algorithms.hihoCoder在线题库练习解答
{
    /*
     对于小Ho面临的这样一个窘境，小Hi沉默了……
     
     -1 -1 -1 -1 -1 -1 -1
     -1 1 1 1 1 1 -1
     -1 1 0 0 0 2 -1
     -1 1 0 0 0 2 -1
     -1 1 1 1 1 1 -1
     -1 -1 -1 -1 -1 -1 -1
     
     但是小Hi毕竟是小Hi，片刻之后他就给出了这样的推理（下用(x, y)表示地图中第x行第y列的格子）：

     “假设(5, 7)是地雷，那么可以通过(5, 6)所标的数字1，判断出(4, 7), (6, 5), (6, 6), (6, 7)4个格子一定不是地雷，
     从而通过(4, 6)所标的数字2，判断出(3, 7)一定为地雷，又已知(2, 6)所标的数字为1，
     可知(1, 5), (1, 6), (1, 7), (2, 7)4个格子一定不是地雷，但是此时却发现：(3, 6)所标的数字为2，
     但是其周围只有可能有一个地雷——所有格子已经探明且只有一个地雷，于是矛盾产生，可知假设错误。

     “由于地图一定有解，可知(5, 7)一定不是地雷，利用一系列的规则可以推断出最后的结果——虽然仍然没有能够完全探明，
     但是仍然知道了比原来多的多的信息。”

     听完小Hi的这一番分析，小Ho点了点头，但是随即又问道：“虽然这样的分析可以解决这一处问题，
     但是又该如何通用的解决所有类似的情况呢？”

     小Hi想了想，道：“之前我们所用的处理扫雷问题的循环是已知地图信息->判断出必定为地雷的格子和必定不为地雷的格子->
     新的地图信息->判断出更多必定为地雷的格子和必定不为地雷的格子，直到不能判断出任何必定为地雷的格子和必定不为地雷的格子。
     这也就意味着没有更多的地图信息，于是这时候就需要我们假设出更多的地图信息，换而言之，就是进行搜索了！

     “那么我们到底该如何进行搜索呢？”小Ho问道。

     小Hi清了清嗓子道：像之前举的那个例子一样，在枚举完一个格子后，立刻利用新获得的地图信息（即这个格子是否为地雷），
     再次使用已知的规则（在这里可以酌情选用，比如我可以只使用前两条规则而不使用第三条规则，毕竟第三条规则一来麻烦，二来耗费时间）
     来尽可能多的获得信息，减少需要枚举的格子的数量，即从一定程度上减少了K的数量（如在之前的例子中就是从22减少到了1），
     从而降低了时间复杂度。

     “有道理！在每获得一点新的信息量之后就通过规则来尝试获取更多的地图信息，可以非常有效的避免冗余的枚举。”小Ho点了点头。

     于是小Hi继续道：“而优化方法二呢，则是优化枚举的顺序——在之前的例子中，为什么我第一个枚举的是(4, 7)这个点，
     而不是其他的点呢？如果我枚举其他的点，又会发生什么事情呢？”

     “好像不会有什么区别呢？”小Ho答道：“在这种情况下，无论枚举哪个格子，都可以利用各种规则判断出其他格子的取值来。”

     小Hi顿时满头黑线……于是解释道：“这是因为在这个例子中标明的数字为1的格子太多的原因啦，不过，
     这也就从侧面说明了选取枚举格子的第一关键要素：尽量枚举那些周围存在较小的标明数字（如1、2）的格子，这样就可以尽快的在枚举之后，
     利用各种规则进行地图信息的计算了。”

     “好吧，虽然勉勉强强，但是也算是一种优化，毕竟如果放着一个数字1不处理而去处理一个数字4，怎么都不是明智的行为呢！”小Ho勉勉强强的点了点头。

     “当然其实一点优化都不加也是可以通过本题的哟！）所以赶紧加油去写吧！”小Hi道。

     “嗯嗯！”小Ho怀着马上能逃出迷宫的热情编写起了程序，但是这一切真的能如他所愿么？
     */
    /*
     输入

     每个测试点（输入文件）存在多组测试数据。

     每个测试点的第一行为一个整数Task，表示测试数据的组数。

     在一组测试数据中：

     第1行为2个整数N，M，表示迷宫的大小。

     接下来的N行，每行M个整数，为一个矩阵A，用以描述整个迷宫，其中对于每一个格子A[i][j]，若A[i][j]=-1，
     则表示该格子没有被探明，若0<=a[i][j]<=8，则表示该格子已经被探明了，且数值代表该格子附近8个格子中的地雷数。<>

     对于100%的数据，满足：5<=N、M<=10, -1<=a[i][j]<=8。<>

     对于100%的数据，满足：符合数据描述的迷宫一定存在。

     对于100%的数据，满足：迷宫中没有探明的格子的数量K<=10。<>

     输出

     对于每组测试数据，输出2个整数，分别为一定为地雷的未探明格子数和一定不为地雷的未探明格子数。
     
     样例输入
        2
        10 10
          0  0  1  2 -1  1  0  0  1 -1
          1  1  1 -1  2  1  0  0  1  1
         -1  1  1  1  1  0  0  0  0  0
          1  1  0  0  0  1  1  1  0  0
          0  0  0  0  0  1 -1  1  0  0
          0  0  0  0  0  2  2  2  0  0
          0  0  0  0  0  1 -1  1  0 -1
          0  0  0  0  0  1  1  1  0  0
          0  0  0  0  1  1  1  0  0  0
         -1  0  0 -1  1 -1  1  0  0  0
        9 10
          0  0  0  0  0 -1  0  0 -1  0
          0  0 -1  0  0  0  0  0  0  0
          0  0  0  0  0  0  0  1  1  1
          0  0  0  0  0  1  2  3 -1  1
          0  0  0  0 -1  1 -1 -1  3  1
          1  1  0  0  0  1  3 -1  2  0
         -1  1  0  0  0  0  1  1 -1  0
          1  1  0  0  0  0  0  0  0  0
          0  0  0  0  0  0  0  0  0  0
        样例输出
        7 3
        5 5
     */
    class SaoLeiLevel3
    {
        private const int DiLei = -999;
        private const int NotDiLei = -100;

        private bool change = true;

        /// <summary>
        /// 主方法——开始
        /// </summary>
        public void Start()
        {
            int groupCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < groupCount; i++)
            {
                int[,] data = Prepare();
                data = LoopGuestAndSolve(data);
                Print(data);
            }
        }

        /// <summary>
        /// 从控制台输入
        /// </summary>
        private int[,] Prepare()
        {
            string row_column = Console.ReadLine();
            int row = int.Parse(row_column.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]);
            int column = int.Parse(row_column.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);

            int[,] data = new int[row, column];
            for (int j = 0; j < row; j++)
            {
                string rowdata = Console.ReadLine();
                string[] rowdatas = rowdata.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int k = 0; k < column; k++)
                {
                    data[j, k] = int.Parse(rowdatas[k]);
                }
            }
            return data;
        }

        /// <summary>
        /// 输出结果
        /// </summary>
        private void Print(int[,] result)
        {
            int dileiCnt = 0;
            int notdileiCnt = 0;
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    if (result[i, j] == DiLei) dileiCnt++;
                    else if (result[i, j] == NotDiLei) notdileiCnt++;
                }
            }
            Console.WriteLine(dileiCnt + " " + notdileiCnt);
        }

        /// <summary>
        /// 循环对每个未知格子进行猜测，
        /// 若猜测结果不会引发矛盾，就认为可以
        /// </summary>
        private int[,] LoopGuestAndSolve(int[,] map)
        {
            bool needBreak = true;

            while (needBreak)
            {
                needBreak = false;

                for (int i = 0; i < map.GetLength(0); i++)
                {
                    if (needBreak) break;
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[i, j] != -1) continue;

                        int[,] copy1 = Copy(map);
                        copy1[i, j] = DiLei;
                        Solve(copy1);
                        if (!IsWrong(copy1) && Diff(map, copy1) > 1)
                        {
                            map = Copy(copy1);
                            needBreak = true;
                            break;
                        }

                        int[,] copy2 = Copy(map);
                        copy2[i, j] = NotDiLei;
                        Solve(copy2);
                        if (!IsWrong(copy2) && Diff(map, copy2) > 1)
                        {
                            map = Copy(copy2);
                            needBreak = true;
                            break;
                        }
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// 比较两个矩阵的区别
        /// </summary>
        private int Diff(int[,] before, int[,] after)
        {
            int diff = 0;
            for (int i = 0; i < before.GetLength(0); i++)
            {
                for (int j = 0; j < before.GetLength(1); j++)
                {
                    if (before[i, j] != after[i, j]) diff++;
                }
            }
            return diff;
        }

        /// <summary>
        /// 处理矩阵
        /// </summary>
        private void Solve(int[,] data)
        {
            change = true;
            while (change)//先按常规不停计算
            {
                change = false;
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        if (data[i, j] == 0)
                        {
                            Find1(data, i, j);
                        }
                        else if (data[i, j] > 0)
                        {
                            Find2(data, i, j);
                            Find3(data, i, j);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 拷贝一个矩阵出来
        /// </summary>
        private int[,] Copy(int[,] data)
        {
            int row = data.GetLength(0);
            int column = data.GetLength(1);
            int[,] copy = new int[row, column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    copy[i, j] = data[i, j];
                }
            }
            return copy;
        }

        /// <summary>
        /// 判断猜测是否有矛盾
        /// </summary>
        private bool IsWrong(int[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    int flag = data[i, j];
                    if (flag >= 0)
                    {
                        int dileiCnt = 0;
                        int unKnownCnt = 0;
                        for (int _i = -1; _i <= 1; _i++)
                        {
                            for (int _j = -1; _j <= 1; _j++)
                            {
                                if (_i == 0 && _j == 0) continue;
                                if (i + _i < 0 || i + _i >= data.GetLength(0)) continue;
                                if (j + _j < 0 || j + _j >= data.GetLength(1)) continue;

                                if (data[i + _i, j + _j] == DiLei) dileiCnt++;
                                if (data[i + _i, j + _j] == -1) unKnownCnt++;
                            }
                        }

                        if (dileiCnt > flag) return true;//实际地雷数>标明的地雷数，错误
                        if (unKnownCnt == 0 && dileiCnt != flag) return true;//所有格子都已探明，实际地雷数不同于标明的地雷数，错误
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 规则一
        /// </summary>
        private void Find1(int[,] data, int i, int j)
        {
            for (int _i = -1; _i <= 1; _i++)
            {
                for (int _j = -1; _j <= 1; _j++)
                {
                    if (_i == 0 && _j == 0) continue;
                    if (i + _i < 0 || i + _i >= data.GetLength(0)) continue;
                    if (j + _j < 0 || j + _j >= data.GetLength(1)) continue;
                    if (data[i + _i, j + _j] != -1) continue;

                    data[i + _i, j + _j] = NotDiLei;
                    change = true;
                }
            }
        }

        /// <summary>
        /// 规则二
        /// </summary>
        private void Find2(int[,] data, int i, int j)
        {
            int unKnownCount = 0;
            int dileiCount = 0;
            for (int _i = -1; _i <= 1; _i++)
            {
                for (int _j = -1; _j <= 1; _j++)
                {
                    if (_i == 0 && _j == 0) continue;
                    if (i + _i < 0 || i + _i >= data.GetLength(0)) continue;
                    if (j + _j < 0 || j + _j >= data.GetLength(1)) continue;

                    if (data[i + _i, j + _j] == -1) unKnownCount++;
                    if (data[i + _i, j + _j] == DiLei) dileiCount++;
                }
            }

            if (unKnownCount != data[i, j] - dileiCount) return;

            for (int _i = -1; _i <= 1; _i++)
            {
                for (int _j = -1; _j <= 1; _j++)
                {
                    if (_i == 0 && _j == 0) continue;
                    if (i + _i < 0 || i + _i >= data.GetLength(0)) continue;
                    if (j + _j < 0 || j + _j >= data.GetLength(1)) continue;
                    if (data[i + _i, j + _j] != -1) continue;

                    data[i + _i, j + _j] = DiLei;
                    change = true;
                }
            }
        }

        /// <summary>
        /// 若周边格子内的地雷数已和中心点标明的相同，但还存在未知格子，
        /// 则这些未知格子一定是安全
        /// 这是我自己扩展的一条规则
        /// </summary>
        private void Find3(int[,] data, int i, int j)
        {
            int count = 0;
            for (int _i = -1; _i <= 1; _i++)
            {
                for (int _j = -1; _j <= 1; _j++)
                {
                    if (_i == 0 && _j == 0) continue;
                    if (i + _i < 0 || i + _i >= data.GetLength(0)) continue;
                    if (j + _j < 0 || j + _j >= data.GetLength(1)) continue;

                    if (data[i + _i, j + _j] == DiLei) count++;
                }
            }

            if (count != data[i, j]) return;

            for (int _i = -1; _i <= 1; _i++)
            {
                for (int _j = -1; _j <= 1; _j++)
                {
                    if (_i == 0 && _j == 0) continue;
                    if (i + _i < 0 || i + _i >= data.GetLength(0)) continue;
                    if (j + _j < 0 || j + _j >= data.GetLength(1)) continue;
                    if (data[i + _i, j + _j] != -1) continue;

                    data[i + _i, j + _j] = NotDiLei;
                    change = true;
                }
            }
        }
    }
}

