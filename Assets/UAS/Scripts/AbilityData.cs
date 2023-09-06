using System;
using System.Collections.Generic;
using UAS.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace UAS
{
    public enum AbilityTargetBehavior
    {
        Target = 1 << 0,
        Point = 1 << 1,
        NoTarget = 1<< 2,
        Passive = 1 << 3,
    }
    
    [CreateAssetMenu(menuName = "UAS/Ability")]
    public class AbilityData: ScriptableObject 
    {
        public string abilityName;
        public AbilityTargetBehavior targetBehavior;
        public string icon;
        public List<EventOnData> events;
        public List<ModifierData> modifiers;
    }

    [Serializable]
    public class EventOnData 
    {
        public string eventName;
        [FormerlySerializedAs("actions")]
        [SerializeReference]
        [SerializeReferenceDropdown]
        public List<EffectData> effects;
    }

    public enum ModifyOp
    {
        Constant,
        Bonus,
        Percent,
        BonusPercent,
    }
    
    [Serializable]
    public struct StatModifierData
    {
        public string statName;
        public float value;
        public ModifyOp op;
    }

    [Serializable]
    public struct StateModifierData
    {
        public string stateName;
    }

    [Serializable]
    public class ModifierData 
    {
        public string name;
        public bool passive;
        public string icon;
        public float duration;
        public float updateInterval;
        public List<EventOnData> events;

        public List<StatModifierData> stats;
        public List<StateModifierData> states;
    }
}