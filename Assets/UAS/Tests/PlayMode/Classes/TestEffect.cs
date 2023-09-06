namespace UAS.Tests
{
    public class TestEffectData : EffectData
    {
        
    }
    public class TestEffect : Effect<TestEffectData>
    {
        public int value = 0;
        public TestEffect(TestEffectData data) : base(data)
        {
        }

        public override void Execute(ref EffectParams effectParams)
        {
            value++;
        }
    }
}