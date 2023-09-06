using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UAS;

namespace Example
{

    [System.Serializable]
    public class HealData : EffectData
    {
        public float amount;
    }

    public class Heal : Effect<HealData>
    {

        public Heal(HealData data) : base(data)
        {
        }

        public override void Execute(ref EffectParams effectParams)
        {
            void Func(RPGUnit target, ref EffectParams effectParams)
            {
                target.Heal(m_Data.amount);
            }

            ForEachTarget(ref effectParams, (ForEachTargetDelegate<RPGUnit>)Func);
        }
    }
}
