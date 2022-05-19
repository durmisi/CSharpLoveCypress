using Xunit;
using Xunit.Abstractions;

namespace CSharpLoveCypress
{
    public class FormTests : TestBase
    {
        public FormTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void SubmitTests()
        {
            // some data initialization code here....
            RunCypressTest("cypress/integration/form/submit.spec.ts");
        }
    }
}