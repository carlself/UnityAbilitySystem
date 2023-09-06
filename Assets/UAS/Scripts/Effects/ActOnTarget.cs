using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UAS
{
    [Serializable]
    public class ActOnTargetData : EffectData
    {
        [SerializeReference]
        [SerializeReferenceDropdown]
        public EffectData effect;
    }
    
    public class ActOnTarget : Effect<ActOnTargetData>
    {
        public ActOnTarget(ActOnTargetData data) : base(data)
        {
        }

        public override void Execute(ref EffectParams effectParams)
        {
            ForEachTargetDelegate<IUnit> func = (IUnit target, ref EffectParams effectParams) =>
            {
                effectParams.target = target;
                Effect effect = EffectFactory.Create(m_Data.effect);
                effect.Execute(ref effectParams);
            };
            
            ForEachTarget(ref effectParams, func);
        }
    }
}