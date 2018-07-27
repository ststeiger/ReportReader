
using System.Linq;


namespace SortAlgorithms.Topological 
{
    
    
    static class Kahn 
    {


        /// <summary>
        /// Topological Sorting (Kahn's algorithm) 
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">All nodes of directed acyclic graph.</param>
        /// <param name="edges">All edges of directed acyclic graph.</param>
        /// <returns>Sorted node in topological order.</returns>
        public static System.Collections.Generic.List<T> Sort<T>(
              System.Collections.Generic.HashSet<T> nodes
            , System.Collections.Generic.HashSet<System.Tuple<T, T>> edges) 
            where T : System.IEquatable<T>
        {
            // Empty list that will contain the sorted elements
            System.Collections.Generic.List<T> L = new System.Collections.Generic.List<T>();

            // Set of all nodes with no incoming edges
            System.Collections.Generic.HashSet<T> S = new System.Collections.Generic.HashSet<T>(
                    nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false) )
            );
            
            // while S is non-empty do
            while (S.Any())
            {
                //  remove a node n from S
                T n = S.First();
                S.Remove(n);

                // add n to tail of L
                L.Add(n);

                // for each node m with an edge e from n to m do
                foreach (System.Tuple<T,T> e in edges.Where(e => e.Item1.Equals(n)).ToList())
                {
                    T m = e.Item2;
                    
                    // remove edge e from the graph
                    edges.Remove(e);

                    // if m has no other incoming edges then
                    if (edges.All(me => me.Item2.Equals(m) == false))
                    {
                        // insert m into S
                        S.Add(m);
                    }

                } // Next e 

            } // Whend 

            // if graph has edges then
            if (edges.Any())
            {
                // throw new System.IO.InvalidDataException("Graph has cycles...");

                // return error (graph has at least one cycle)
                return null;
            }
            
            // return L (a topologically sorted order)
            return L;
        } // End Function Sort 


    } // End Class Kahn 


} // End Namepace 
