using System;
using System.Collections.Generic;

namespace Algorithms.hihoCoder在线题库练习解答
{
    /// <summary>
    /// 给定一个只包含大写字母"ABC"的字符串s，消除过程是如下进行的：
    /// 1)如果s包含长度超过1的由相同字母组成的子串，那么这些子串会被同时消除，余下的子串拼成新的字符串。
    /// 例如"ABCCBCCCAA"中"CC","CCC"和"AA"会被同时消除，余下"AB"和"B"拼成新的字符串"ABB"。
    /// 2)上述消除会反复一轮一轮进行，直到新的字符串不包含相邻的相同字符为止。例如”ABCCBCCCAA”经过一轮消除得到"ABB"，再经过一轮消除得到"A"
    /// 在消除开始前需要在s中任意位置(第一个字符之前、最后一个字符之后以及相邻两个字符之间)插入任意一个字符('A','B'或者'C')，得到字符串t。
    /// t经过一系列消除后，得分是消除掉的字符的总数。
    /// 请计算最高得分
    /// 输入：
    /// 3
    /// ABCBCCCAA
    /// AAA
    /// ABC
    /// 输出：
    /// 9
    /// 4
    /// 2
    /// </summary>
    public class StringClearup
    {
        public static void Start()
        {
            int stringCount = int.Parse(Console.ReadLine());
            string[] stringList = new string[stringCount];
            for (int i = 0; i < stringCount; i++)
                stringList[i] = Console.ReadLine();

            for (int i = 0; i < stringList.Length; i++)
            {
                string[] list = Insert(stringList[i]);
                int max = -1;

                foreach (string s in list)
                {
                    int j = 0;
                    string last = s;

                    while (true)
                    {
                        string after = RemoveSameChar(last);
                        int removeLength = last.Length - after.Length;
                        if (removeLength == 0)
                        {
                            j = s.Length - after.Length;
                            if (j > max) max = j;
                            break;
                        }
                        else
                        {
                            last = after;
                        }
                    }
                }

                Console.WriteLine(max);
            }

            Console.ReadLine();
        }

        private static string[] Insert(string s)
        {
            string[] Alpha = new string[3] { "A", "B", "C" };
            List<string> result = new List<string>();

            for (int i = 1; i <= s.Length + 1; i++)
            {
                for (int j = 0; j < Alpha.Length; j++)
                {
                    result.Add(s.Insert(i - 1, Alpha[j]));
                }
            }
            return result.ToArray();
        }

        private static string RemoveSameChar(string s)
        {
            List<Tuple<int, int>> position = new List<Tuple<int, int>>();

            int j = -1;
            for (int i = 0; i < s.Length - 1; i = j)
            {
                if (s[i] != s[i + 1])
                {
                    j = i + 1;
                    continue;
                }

                j = i + 1;
                for (; j < s.Length; j++)
                {
                    if (s[i] != s[j]) break;
                }

                position.Add(Tuple.Create(i, j - 1));
            }

            if (position.Count == 0) return s;
            char[] c = s.ToCharArray();
            foreach (var t in position)
            {
                for (int i = t.Item1; i <= t.Item2; i++)
                {
                    c[i] = '_';
                }
            }
            string result = "";
            foreach (var cc in c)
            {
                if (cc != '_') result += cc;
            }
            return result;
        }
    }
}
