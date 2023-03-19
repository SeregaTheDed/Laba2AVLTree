using AVLTree;
using System.Diagnostics;

namespace Laba2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new();
            HashSet<int> hashSet = new HashSet<int>();
            while (hashSet.Count < 1000000)
            {
                hashSet.Add(rnd.Next());
            }
            var array = hashSet.ToArray();

            var tree = new AVLTree<int, int>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < array.Length; i++)
            {
                tree.Add(array[i], array[i]);
            }
            for (int i = 500000; i < 700000; i++)
            {
                tree.Remove(array[i]);
            }
            for (int i = 0; i < array.Length; i++)
            {
                tree.Contains(array[i]);
            }
            stopwatch.Stop();
            Console.WriteLine("AVLTree: " + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();

            SortedDictionary<int, int> sD = new SortedDictionary<int, int>();
            stopwatch.Start();
            for (int i = 0; i < array.Length; i++)
            {
                sD.Add(array[i], array[i]);
            }
            for (int i = 500000; i < 700000; i++)
            {
                sD.Remove(array[i]);
            }
            for (int i = 0; i < array.Length; i++)
            {
                sD.ContainsKey(array[i]);
            }
            stopwatch.Stop();
            Console.WriteLine("SortedDictionary: " + stopwatch.ElapsedMilliseconds);


            //var tree = new AVLTree<int, int>();
            //for (int i = 0; i < 10; i++)
            //{
            //    tree.Add(i, i);
            //}
            //TreePrinter<int, int>.Print(tree._root);
            //tree.Remove(4);
            //TreePrinter<int, int>.Print(tree._root);
            //tree.Remove(1);
            //TreePrinter<int, int>.Print(tree._root);
            //tree.Remove(2);
            //TreePrinter<int, int>.Print(tree._root);
            //tree.Remove(7);
            //TreePrinter<int, int>.Print(tree._root);
            //Console.WriteLine("-----------");
            //for (int i = 3; i < 7; i++)
            //{
            //    tree.Remove(i);
            //}
            //TreePrinter<int, int>.Print(tree._root);

            /*var tree = new AVLTree<int, int>();
            tree.Add(20, 20);
            tree.Add(10, 10);
            tree.Add(30, 30);
            tree.Add(80, 80);
            tree.Add(40, 40);
            TreePrinter<int, int>.Print(tree._root);
            tree.Add(60, 60);
            TreePrinter<int, int>.Print(tree._root);
            tree.Add(50, 50);
            tree.Add(70, 70);
            TreePrinter<int, int>.Print(tree._root);
            tree.Remove(20);
            TreePrinter<int, int>.Print(tree._root);
            tree.Remove(30);
            TreePrinter<int, int>.Print(tree._root);
            Console.WriteLine(tree.Count);
            Console.WriteLine(tree.Remove(60));
            TreePrinter<int, int>.Print(tree._root);*/
        }
    }
}