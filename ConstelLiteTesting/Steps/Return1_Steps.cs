using System;
using TechTalk.SpecFlow;

namespace ConstelLiteTesting.Steps
{
    [Binding]
    public class Return1_ReturnSingleVariableCorrectReturnOfValuesAccordingToTheirTypeSteps
    {
        [Then(@"a SyntaxError should be raised at compile time: UndefinedVariable")]
        public void ThenASyntaxErrorShouldBeRaisedAtCompileTimeUndefinedVariable()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
