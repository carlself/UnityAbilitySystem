using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace UAS
{
    public abstract class TargetData
    {
        public abstract void FindTargets(List<IUnit> results, ref EffectParams effectParams);
    }

    public enum EffectTargetType
    {
        Caster = 0,
        Target = 1,
        Point = 2,
        Attacker = 3,
        Unit = 4,
        Projectile = 5,
    }

    [Serializable]
    public class SingleTargetData : TargetData
    {
        public EffectTargetType targetType;

        public override void FindTargets(List<IUnit> results, ref EffectParams effectParams)
        {
            switch (targetType)
            {
                case EffectTargetType.Caster:
                {
                    if (effectParams.caster != null)
                    {
                        results.Add(effectParams.caster);
                    }
                    break;
                }
                    
                case EffectTargetType.Target:
                {
                    if (effectParams.target != null)
                    {
                        results.Add(effectParams.target);
                    }
                    break;
                }
            }
        }
    }

    [Serializable]
    public class MultipleTargetData : TargetData
    {
        public EffectTargetType center;
        public float radius;
        
        public override void FindTargets(List<IUnit> results, ref EffectParams effectParams)
        {
            
        }
    }
}