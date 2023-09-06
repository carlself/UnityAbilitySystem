using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UAS
{
    [Serializable]
    public abstract class EffectData 
    {
        public string name;
        [SerializeReference]
        [SerializeReferenceDropdown]
        public TargetData target;
    }
    
    public abstract class Effect
    {
        public abstract void Execute(ref EffectParams effectParams);
    }
    
    public abstract class Effect<T> : Effect where T:EffectData
    {
        protected T m_Data;

        public Effect(T data)
        {
            m_Data = data;
        }

        public delegate void ForEachTargetDelegate<in TTarget>(TTarget target, ref EffectParams effectParams) where TTarget : IUnit;
        protected void ForEachTarget<TTarget>(ref EffectParams effectParams, ForEachTargetDelegate<TTarget> func)
            where TTarget:class, IUnit
        {
            if(m_Data.target == null)
                return;
            
            var targets = ListPool<IUnit>.Get();
            m_Data.target.FindTargets(targets, ref effectParams);
            foreach (var target in targets)
            {
                func(target as TTarget, ref effectParams);
            }
            ListPool<IUnit>.Release(targets);
        }
    }
}
