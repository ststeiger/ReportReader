
using System.Linq;


namespace SortAlgorithms.Topological.Test
{
    

    public class KahnTests 
    {


        private static void BaseTestInteger()
        {
            //
            // digraph G {
            //   "7"  -> "11"
            //   "7"  -> "8"
            //   "5"  -> "11"
            //   "3"  -> "8"
            //   "3"  -> "10"
            //   "11" -> "2"
            //   "11" -> "9"
            //   "11" -> "10"
            //   "8"  -> "9"
            // }

            System.Collections.Generic.List<int> ret = Kahn.Sort(
                new System.Collections.Generic.HashSet<int>(new[] { 7, 5, 3, 8, 11, 2, 9, 10 }),
                new System.Collections.Generic.HashSet<System.Tuple<int, int>>(
                    new[]
                    {
                        System.Tuple.Create(7, 11),
                        System.Tuple.Create(7, 8),
                        System.Tuple.Create(5, 11),
                        System.Tuple.Create(3, 8),
                        System.Tuple.Create(3, 10),
                        System.Tuple.Create(11, 2),
                        System.Tuple.Create(11, 9),
                        System.Tuple.Create(11, 10),
                        System.Tuple.Create(8, 9)
                    }
                )
            );

            System.Console.WriteLine(ret);
            System.Diagnostics.Debug.Assert(ret.SequenceEqual(new[] { 7, 5, 11, 2, 3, 8, 9, 10 }));
        } // End Sub BaseTestInteger 


        public static void BaseTestString()
        {
            System.Collections.Generic.List<string> ret = Kahn.Sort(
                new System.Collections.Generic.HashSet<string>(new[] { "A", "B", "C", "D", "E", "F", "G", "H" }),
                new System.Collections.Generic.HashSet<System.Tuple<string, string>>(
                    new[]
                    {
                        System.Tuple.Create("A", "E"),
                        System.Tuple.Create("A", "D"),
                        System.Tuple.Create("B", "E"),
                        System.Tuple.Create("C", "D"),
                        System.Tuple.Create("C", "H"),
                        System.Tuple.Create("E", "F"),
                        System.Tuple.Create("E", "G"),
                        System.Tuple.Create("E", "H"),
                        System.Tuple.Create("D", "G")
                    }
                )
            );

            System.Console.WriteLine(ret);

            System.Diagnostics.Debug.Assert(ret.SequenceEqual(new[] { "A", "B", "E", "F", "C", "D", "G", "H" }));
            System.Console.WriteLine("Finished");
        } // End Sub BaseTestString 


        public static void TestMe()
        {
            // ls.Add(new TopologicObject("bar"));
            // ls.Add(new TopologicObject("foo"));
            // ls.Add(new DB.Topology.TopologicObject("foobar").DependsOn("foo").DependsOn("bar") );
            // ls.Add(new TopologicObject("omg").DependsOn("foo"));

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            System.Collections.Generic.List<string> ret = Kahn.Sort(
                new System.Collections.Generic.HashSet<string>(new string[] { "bar", "foo", "foobar", "omg" }),
                new System.Collections.Generic.HashSet<System.Tuple<string, string>>(
                    new[]
                    {
                         // System.Tuple.Create("foobar", "foo") 
                         //,System.Tuple.Create("foobar", "bar")
                         //,System.Tuple.Create("omg", "foo")

                         // System.Tuple.Create(Dependency, DependencyOwner);
                         System.Tuple.Create("foo", "foobar")
                        ,System.Tuple.Create("bar", "foobar")
                        ,System.Tuple.Create("foo", "omg")
                    }
                )
            );

            sw.Stop();
            long ems = sw.ElapsedMilliseconds;
            System.Console.WriteLine($"Duration Kahn:\t{ems}");
        } // End Sub TestMe 


        public static void TestWithWrapper()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            System.Collections.Generic.Dictionary<string, int> dict =
                new System.Collections.Generic.Dictionary<string, int>(
                System.StringComparer.InvariantCultureIgnoreCase
            );

            System.Collections.Generic.List<TopologicObject> ls =
                new System.Collections.Generic.List<TopologicObject>();

            ls.Add(new TopologicObject("bar"));
            ls.Add(new TopologicObject("foo"));

            ls.Add(new TopologicObject("foobar")
                    .DependsOn("foo").DependsOn("bar")
            );

            ls.Add(new TopologicObject("omg").DependsOn("foo"));


            for (int i = 0; i < ls.Count; ++i)
            {
                ls[i].Index = i;
                dict.Add(ls[i].Name, i);
            } // Next i 


            for (int i = 0; i < ls.Count; ++i)
            {
                for (int j = 0; j < ls[i].Dependencies.Count; ++j)
                {
                    int key = dict[ls[i].Dependencies[j].Name];
                    ls[i].Dependencies[j].Index = key;
                } // Next j 

            } // Next i 



            System.Collections.Generic.HashSet<int> hsAllObjects =
                new System.Collections.Generic.HashSet<int>();

            System.Collections.Generic.HashSet<System.Tuple<int, int>> hsDependence =
                new System.Collections.Generic.HashSet<System.Tuple<int, int>>();


            for (int i = 0; i < ls.Count; ++i)
            {
                hsAllObjects.Add(i);
            } // Next i 

            for (int i = 0; i < ls.Count; ++i)
            {
                for (int j = 0; j < ls[i].Dependencies.Count; ++j)
                {
                    hsDependence.Add(System.Tuple.Create(ls[i].Dependencies[j].Index, i));
                } // Next j 

            } // Next i 

            System.Collections.Generic.List<int> ret = Kahn.Sort(hsAllObjects, hsDependence);
            string[] sret = new string[ret.Count];
            for (int i = 0; i < ret.Count; ++i)
            {
                sret[i] = ls[ret[i]].Name;
            } // Next i 


            sw.Stop();
            long durKahn = sw.ElapsedMilliseconds;

            // sret = sret.Reverse().ToArray();
            System.Console.WriteLine(sret);

            // And the winner is: NOT KAHN !
            System.Console.WriteLine("{0}, {1}", durKahn);
        } // End Sub TestWithWrapper 


    } // End Class KahnTests 




    public class MatrixTests
    {


        public static void MatrixSorterTest()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            System.Collections.Generic.Dictionary<string, int> dict =
                new System.Collections.Generic.Dictionary<string, int>(
                System.StringComparer.InvariantCultureIgnoreCase
            );

            System.Collections.Generic.List<TopologicObject> ls =
                new System.Collections.Generic.List<TopologicObject>();

            ls.Add(new TopologicObject("bar"));
            ls.Add(new TopologicObject("foo"));

            ls.Add(new TopologicObject("foobar")
                    .DependsOn("foo").DependsOn("bar")
            );

            ls.Add(new TopologicObject("omg").DependsOn("foo"));


            for (int i = 0; i < ls.Count; ++i)
            {
                ls[i].Index = i;
                dict.Add(ls[i].Name, i);
            } // Next i 


            for (int i = 0; i < ls.Count; ++i)
            {
                for (int j = 0; j < ls[i].Dependencies.Count; ++j)
                {
                    int key = dict[ls[i].Dependencies[j].Name];
                    ls[i].Dependencies[j].Index = key;
                } // Next j 

            } // Next i 


            MatrixSorter sorter = new MatrixSorter(ls.Count);


            for (int i = 0; i < ls.Count; ++i)
            {
                sorter.AddVertex(i);
            } // Next i 

            for (int i = 0; i < ls.Count; ++i)
            {
                for (int j = 0; j < ls[i].Dependencies.Count; ++j)
                {
                    sorter.AddEdge(ls[i].Dependencies[j].Index, i); // i depends on Dependency[j] 
                } // Next j 

            } // Next i 

            int[] res = sorter.Sort();

            string[] sres = new string[res.Length];
            for (int i = 0; i < sres.Length; ++i)
            {
                sres[i] = ls[res[i]].Name;
            } // Next i 

            sw.Stop();
            long ems = sw.ElapsedMilliseconds;
            System.Console.WriteLine($"Duration Matrix:\t{ems}");

            // res = res.Reverse().ToArray();
            // System.Console.WriteLine(sres);
        } // End Sub MatrixSorterTest 


        public static void SimpleMatrixSorterTest()
        {
            MatrixSorter sorter = new MatrixSorter(5);
            sorter.AddVertex(0);
            sorter.AddVertex(1);
            sorter.AddVertex(2);
            sorter.AddVertex(3);
            sorter.AddVertex(4);

            sorter.AddEdge(2, 3); // 3 hängt von 2 ab
            sorter.AddEdge(0, 1); // 1 hängt von 0 ab

            int[] res = sorter.Sort();
            // res = res.Reverse().ToArray();
            System.Console.WriteLine(res);
        } // End Sub SimpleMatrixSorterTest 


    }


}