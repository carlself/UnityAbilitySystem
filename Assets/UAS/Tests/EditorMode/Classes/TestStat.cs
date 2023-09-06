using NUnit.Framework.Internal;

namespace UAS.Tests
{
    public class TestStat : Stat
    {
        public TestStat(float value) : base(value)
        {
        }

        public override string GetName()
        {
            return nameof(TestStat);
        }
    }
}