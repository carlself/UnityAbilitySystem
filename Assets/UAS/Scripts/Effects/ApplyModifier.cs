using System;

namespace UAS
{
    [Serializable]
    public class ApplyModifierData : EffectData
    {
        public float duration;
        public string modifierName;
        // public ModifierData modifier;
    }
    
    public class ApplyModifier : Effect<ApplyModifierData>
    {
        public ApplyModifier(ApplyModifierData data) : base(data)
        {
        }

        public override void Execute(ref EffectParams effectParams)
        {
            ForEachTargetDelegate<IUnit> func = (IUnit target, ref EffectParams effectParams) =>
            {
                target.AddNewModifier( m_Data.modifierName, m_Data.duration, effectParams.caster, effectParams.ability);
            };
            
            ForEachTarget(ref effectParams, func);
        }
    }
}