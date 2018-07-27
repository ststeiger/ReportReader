
namespace Fw4csReportExecution.Trash
{


    class MyGraph
    {


        // Dependencies of tables, views, functions 
        public class EntityOperations 
        {
            protected string m_currentEntity;
            public MyGraph Graph;
            

            public EntityOperations(MyGraph graph, string x)
            {
                this.Graph = graph;
                this.m_currentEntity = x;
            }


            public EntityOperations And(string b)
            {
                Graph.AddDependency(this.m_currentEntity, b);
                return this;
            }


            public EntityOperations DependsOn(string b)
            {
                Graph.AddDependency(this.m_currentEntity, b);
                return this;
            }


        } // End Class EntityOperations 


        private void AddDependency(string entity, string dependency)
        {
            // AddEdge(entity, dependency);
        }


        public EntityOperations Entity(string a)
        {
            return new EntityOperations(this, a);
        }

        public static void Test()
        {
            MyGraph graph = new MyGraph();
            graph.Entity("taskunion").DependsOn("gb,so").And("gs");
        }


    } // End Class MyGraph 


} // End Namespace 
