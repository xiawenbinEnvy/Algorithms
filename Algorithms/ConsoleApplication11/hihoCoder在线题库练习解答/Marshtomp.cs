using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Algorithms.hihoCoder在线题库练习解答
{
    /*
     fjxmlhx每天都在被沼跃鱼刷屏，因此他急切的找到了你希望你写一个程序屏蔽所有句子中的沼跃鱼(“marshtomp”，不区分大小写)。
     为了使句子不缺少成分，统一换成 “fjxmlhx” 。

     输入
     输入包括多行。
     每行是一个字符串，长度不超过200。
     一行的末尾与下一行的开头没有关系。

     输出
     输出包含多行，为输入按照描述中变换的结果。

     样例输入
     The Marshtomp has seen it all before.
     marshTomp is beaten by fjxmlhx!
     AmarshtompB
     样例输出
     The fjxmlhx has seen it all before.
     fjxmlhx is beaten by fjxmlhx!
     AfjxmlhxB
     */
    class Marshtomp
    {
        public static void Start()
        {
            string s = "";
            while ((s = Console.ReadLine()) != null)
            {
                Console.WriteLine(Replace(s));
            }
            Console.ReadLine();
        }

        /// <summary>
        /// 替换子字符串
        /// </summary>
        private static string Replace(string s)
        {
            int start;
            string txt = s;
            while ((start = BoyerMoore(txt, "marshtomp")) > -1)
            {
                int end = start + 8;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < txt.Length; i++)
                {
                    if (i < start)
                    {
                        sb.Append(txt[i]);
                    }
                    else
                    {
                        sb.Append("fjxmlhx");
                        for (int j = end + 1; j < txt.Length; j++)
                        {
                            sb.Append(txt[j]);
                        }
                        txt = sb.ToString();
                        break;
                    }
                }
            }
            return txt;
        }

        /// <summary>
        /// BoyerMoore子字符串查找算法
        /// </summary>
        private static int BoyerMoore(string txt, string pat)
        {
            txt = txt.ToLower();

            var charUseCount = new Dictionary<char, int>();
            for (int i = 0; i < pat.Length; i++)
            {
                charUseCount[pat[i]] = i;
            }

            int N = txt.Length;
            int M = pat.Length;

            int skip = 0;
            for (int i = 0; i <= N - M; i += skip)
            {
                skip = 0;
                for (int j = M - 1; j >= 0; j--)
                {
                    if (txt[i + j] != pat[j])
                    {
                        if (charUseCount.ContainsKey(txt[i + j]))
                            skip = j - charUseCount[txt[i + j]];
                        else
                            skip = j + 1;

                        if (skip <= 0) skip = 1;
                        break;
                    }
                }
                if (skip == 0) return i;
            }

            return -1;
        }
    }
}
