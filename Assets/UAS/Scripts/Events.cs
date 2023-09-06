namespace UAS
{
    public interface IModifierEvent
    {
        void FillActionParams(ref EffectParams effectParams);
    }

    public struct OnUpdate : IModifierEvent
    {
        public void FillActionParams(ref EffectParams effectParams)
        {
            
        }
    }
}