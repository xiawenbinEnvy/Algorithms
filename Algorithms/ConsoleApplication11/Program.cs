using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ConsoleApplication11
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(@"d:\Users\wbxia\Desktop\Leipzig1M.txt", Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sb.Append(line).Append(" ");
            }
            var list = sb.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            RedBlackBinarySearchTree<string, string> tree = null;
            //Task t1 = Task.Factory.StartNew(() => { Parallel.ForEach(list, s => tree.Put(s, s)); });
            //Task t2 = Task.Factory.StartNew(() => Parallel.For(0, 100000, a => { tree.Get("Japan"); }));
            //Task.WaitAll(t1);
            for (int i = 0; i < 5; i++)
            {
                tree = new RedBlackBinarySearchTree<string, string>();
                foreach (var s in list) tree.Put(s, s);
                Console.WriteLine(tree.N);
            }

            Console.ReadLine();
        }
    }
}

