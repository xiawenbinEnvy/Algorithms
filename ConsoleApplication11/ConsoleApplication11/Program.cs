using System;

namespace ConsoleApplication11
{
    class Program
    {
        static void Main(string[] args)
        {
            NFA nfa = new NFA("(((AD){2}DR)+)");
            Console.WriteLine(nfa.recognizes("ADADDRADADDRADADDRADADDRADADDRADADDRADADDRADADDRADADDR"));
            //Console.WriteLine(nfa.recognizes("ACD"));

            Console.ReadLine();
        }
    }
}

