using System;
using UAS;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Example
{
    [Serializable]
    public class CreateProjectileData : EffectData
    {
        public float speed;
        public float damage;
    }
    
    public class CreateProjectile : Effect<CreateProjectileData>
    {
        public CreateProjectile(CreateProjectileData data) : base(data)
        {
        }

        public override void Execute(ref EffectParams effectParams)
        {
            void Func(RPGUnit target, ref EffectParams effectParams)
            {
                RPGUnit caster = effectParams.caster as RPGUnit;
                var projectile = ProjectileManager.Instance.SpawnProjectile(m_Data.speed, 
                    caster.headTransform.position, target);
                projectile.damage = m_Data.damage;
            }

            ForEachTarget(ref effectParams, (ForEachTargetDelegate<RPGUnit>)Func);
        }
    }
}