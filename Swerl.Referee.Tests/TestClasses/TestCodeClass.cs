namespace Swerl.Referee.UnitTests.TestClasses
{
    public class TestCodeClass
    {
        public bool SomethingDone { get; set; }
        public string Param { get; set; }

        public void DoSomething(string param)
        {
            SomethingDone = true;
            Param = "test";
        }
    }

    public class TestCodeClass2
    {
        public bool SomethingDone { get; set; }
        public string Param { get; set; }

        public void DoSomething(string param)
        {
            SomethingDone = true;
            Param = "test";
        }
    }
}
