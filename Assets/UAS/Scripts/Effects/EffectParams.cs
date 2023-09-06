using UnityEngine;

namespace UAS
{
    public struct EffectParams
    {
        public IAbility ability;
        public Modifier modifier;
        public IUnit caster;
        public IUnit target;
        public Vector3? point;
    }
}