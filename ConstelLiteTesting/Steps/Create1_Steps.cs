using TechTalk.SpecFlow;
using ConstelLite;
using FluentAssertions;

namespace ConstelLiteTesting.Steps
{
    [Binding]
    public class Create1_Steps
    {
        private readonly GraphEngine graphEngine = new GraphEngine();
        //private readonly Graph graph; //= Graph.GetInstance();

        [Given(@"an empty graph")]
        public void GivenAnEmptyGraph()
        {
            graphEngine.NewGraph();
        }
        
        [When(@"executing query: ""(.*)""")]
        public void WhenExecutingQuery(string query)
        {
            graphEngine.ExecuteQuery(query);
        }
        
        [Then(@"the node size should be: (.*)")]
        public void ThenTheNodeSizeShouldBe(int size)
        {
            Graph.Size.Should().Be(size);
        }
    }
}
