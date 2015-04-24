using System;
using System.Collections.Generic;

namespace Algorithms.hihoCoder在线题库练习解答
{
    /*
     描述
     Little Hi gets lost in the city. He does not know where he is. He does not know which direction is north.
     Fortunately, Little Hi has a map of the city. The map can be considered as a grid of N*M blocks. 
     Each block is numbered by a pair of integers. The block at the north-west corner is (1, 1) and the one at the south-east corner is (N, M). 
     Each block is represented by a character, describing the construction on that block:
     '.' for empty area, 'P' for parks, 'H' for houses, 'S' for streets, 'M' for malls, 'G' for government buildings, 'T' for trees and etc.
     Given the blocks of 3*3 area that surrounding Little Hi(Little Hi is at the middle block of the 3*3 area), please find out the position of him. Note that Little Hi is disoriented, the upper side of the surrounding area may be actually north side, south side, east side or west side.

     输入
     Line 1: two integers, N and M(3 <= N, M <= 200).
     Line 2~N+1: each line contains M characters, describing the city's map. The characters can only be 'A'-'Z' or '.'.
     Line N+2~N+4: each line 3 characters, describing the area surrounding Little Hi.

     输出
     Line 1~K: each line contains 2 integers X and Y, indicating that block (X, Y) may be Little Hi's position. 
     If there are multiple possible blocks, output them from north to south, west to east.

     样例输入
     8 8
     ...HSH..
     ...HSM..
     ...HST..
     ...HSPP.
     PPGHSPPT
     PPSSSSSS
     ..MMSHHH
     ..MMSH..
     SSS
     SHG
     SH.
     样例输出
     5 4
     */

    class LostInTheCity
    {
        /// <summary>
        /// 主流程
        /// </summary>
        public static void Start()
        {
            var data = Prepare();
            string[,] map = data.Item1;
            string[,] surrounding = data.Item2;

            int N = map.GetLength(0);
            int M = map.GetLength(1);

            List<string> result = new List<string>();

            for (int i = 0; i <= N - 3; i++)
            {
                for (int j = 0; j <= M - 3; j++)
                {
                    string[,] mapPart = CuttingMap(map, i, j);
                    if (IsMatch(mapPart, surrounding))
                    {
                        result.Add(string.Format("{0} {1}", i + 2, j + 2));
                    }
                }
            }

            foreach (string s in result)
                Console.WriteLine(s);

            Console.ReadLine();
        }
        /// <summary>
        /// 从控制台获取输入，生成题目数据
        /// </summary>
        private static Tuple<string[,], string[,]> Prepare()
        {
            string N_M = Console.ReadLine();
            string[] N_M_Split = N_M.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            int N = int.Parse(N_M_Split[0]);
            int M = int.Parse(N_M_Split[1]);

            string[,] map = new string[N, M];

            for (int n = 0; n < N; n++)
            {
                string mapRow = Console.ReadLine();
                for (int m = 0; m < M; m++)
                {
                    map[n, m] = mapRow[m].ToString();
                }
            }

            string[,] surrounding = new string[3, 3];

            for (int n = 0; n < 3; n++)
            {
                string surroundingRow = Console.ReadLine();
                for (int m = 0; m < 3; m++)
                {
                    surrounding[n, m] = surroundingRow[m].ToString();
                }
            }

            return Tuple.Create(map, surrounding);
        }

        /// <summary>
        /// 切割地图，返回地图中的3*3的一小块
        /// </summary>
        private static string[,] CuttingMap(string[,] map, int n, int m)
        {
            int N = map.GetLength(0);
            int M = map.GetLength(1);

            string[,] mapPart = new string[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mapPart[i, j] = map[n + i, m + j];
                }
            }
            return mapPart;
        }

        /// <summary>
        /// 判断这块地图区域是否和环境数据符合，输出符合的坐标
        /// </summary>
        private static bool IsMatch(string[,] map, string[,] surrounding)
        {
            int mapCount = 0;
            bool[,] mark = new bool[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    string k = map[i, j];
                    bool needBreak = false;

                    for (int _i = 0; _i < 3; _i++)
                    {
                        if (needBreak) break;
                        for (int _j = 0; _j < 3; _j++)
                        {
                            if (surrounding[_i, _j] == k && !mark[_i, _j])
                            {
                                needBreak = true;
                                mark[_i, _j] = true;
                                mapCount++;
                                break;
                            }
                        }
                    }
                }
            }

            return mapCount == 9;
        }
    }
}
