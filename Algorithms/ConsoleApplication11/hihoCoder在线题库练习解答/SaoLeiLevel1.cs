using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.hihoCoder在线题库练习解答
{
    /*
     在一个大小为2*N的广场，其中第一行里的某一些格子里可能会有至多一个地雷，
     而第二行的格子里全都为数字，表示第一行中距离与这个格子不超过2的格子里总共有多少个地雷，
     即第二行的第i个格子里的数字表示第一行的第i-1个, 第i个, 第i+1个，三个格子（如果i=1或者N则不一定有三个）里的地雷的总数。
     而我们要做的是——找出哪些地方一定是雷，哪些地方一定不是雷
     输入
     每个测试点（输入文件）存在多组测试数据。每个测试点的第一行为一个整数Task，表示测试数据的组数。在一组测试数据中：
     第1行为1个整数N，表示迷宫的宽度。
     第2行为N个整数A_1 ... A_N，依次表示迷宫第二行的N个格子里标注的数字。
     对于100%的数据，满足1<=N<=10^5, 0<=a_i<=3.<>
     对于100%的数据，满足符合数据描述的地图一定存在。
     输出
     对于每组测试数据，输出2行，其中第一行先输出一定为地雷的格子的数量，
     然后按照从小到大的顺序输出所有一定为地雷的格子的位置，第二行先输出一定不为地雷的格子的数量，
     按照从小到大的顺序输出所有一定不为地雷的格子的位置。

     样例输入
     2
     3
     1 1 1
     10
     1 2 1 2 2 3 2 2 2 2 
     样例输出
     1 2
     2 1 3
     7 1 3 5 6 7 9 10 
     3 2 4 8 
     */
    class SaoLeiLevel1
    {
        /// <summary>
        /// 从Console获取输入
        /// </summary>
        private static void Prepare()
        {
            int groupCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < groupCount; i++)
            {
                int count = int.Parse(Console.ReadLine());
                int[,] map = new int[2, count];
                string number = Console.ReadLine();
                string[] numbers = number.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < count;j++ )
                {
                    map[0, j] = -1;//-1表示未知
                    map[1, j] = int.Parse(numbers[j]);
                }
            }
        }

        private static bool Recognition(int[,] map, int index,int length)
        {
            if (index == length) return true;
            int actual = 0;
            if (index == 0)
            {
                actual = map[0, index] + map[0, index + 1];
            }
            else if (index == length - 1)
            {
                actual = map[0, index - 1] + map[0, index];
            }
            else
            {
                actual = map[0, index - 1] + map[0, index] + map[0, index + 1];
            }
            if (actual > map[1, index])
            {
                return false;
            }
            int canSet = map[1, index] - actual;
            if (canSet > 0)
            {
                //copy，设置猜测
                return Recognition(map, index + 1, length);
            }
        }

        private static int[,] Copy(int[,] map, int index, int length)
        {
            int[,] mapCopy = new int[2, length];
            for (int i = 0; i < length; i++)
            {
                if (i == index)
                    mapCopy[0, i] = 1;
                else
                    mapCopy[0, i] = map[0, i];

                mapCopy[1, i] = map[1, i];
            }
            return mapCopy;
        }
    }
}
