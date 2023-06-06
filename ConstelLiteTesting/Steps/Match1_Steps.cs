using TechTalk.SpecFlow;
using ConstelLite;

namespace ConstelLiteTesting.Steps
{
    [Binding]
    public class Match1_Steps
    {
        private readonly GraphEngine graphEngine = new GraphEngine();

        [Given(@"having executed: ""(.*)""")]
        public void GivenHavingExecuted(string query)
        {
            graphEngine.ExecuteQuery(query);
        }
        
        [Then(@"the result should be, in any order:")]
        public void ThenTheResultShouldBeInAnyOrder(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
