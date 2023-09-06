using System;
using System.Collections;
using System.Collections.Generic;
using UAS.Stats;
using UnityEngine;

namespace UAS
{
    public interface IUnit
    {
        Modifier AddNewModifier( string modifierName, float duration, IUnit caster, IAbility ability);
        void AddModifier(Modifier modifier);
        void RemoveModifier(Modifier modifier);
        void RemoveModifierByName(string modifierName);
        bool HasModifier(string modifierName);

        Coroutine ScheduleCoroutine(IEnumerator coroutine);
        void UnScheduleCoroutine(Coroutine coroutine);
    }
    
    public class Unit<TStateFlag> : MonoBehaviour, IUnit where TStateFlag:unmanaged, Enum
    {
        protected List<Modifier> m_Modifiers = new();
        protected StatContainer m_StatContainer = new();
        public StatContainer StatContainer => m_StatContainer;
        protected StateContainer<TStateFlag> m_StateContainer = new();
        public StateContainer<TStateFlag> StateContainer => m_StateContainer;
        
        public Modifier AddNewModifier(string modifierName, float duration, IUnit caster, IAbility ability)
        {
            if (ability == null)
            {
                Debug.LogError("Fail to create modifier. Ability is required");
                return null;
            }
            
            var modifier = ability.CreateModifier(modifierName, duration, caster, this);
            AddModifier(modifier);

            return modifier;
        }

        public void AddModifier(Modifier modifier)
        {
            m_Modifiers.Add(modifier);
            
            m_StatContainer.AddModifier(modifier);
            m_StateContainer.AddModifier(modifier);
        }


        public void RemoveModifier(Modifier modifier)
        {
            m_Modifiers.Remove(modifier);
            m_StatContainer.RemoveModifier(modifier);
            m_StateContainer.RemoveModifier(modifier);
            modifier.Destroy();
        }

        public void RemoveModifierByName(string modifierName)
        {
            Modifier modifier = m_Modifiers.Find(m => m.Name == modifierName);
            if (modifier != null)
            {
                RemoveModifier(modifier);
            }
        }

        public bool HasModifier(string modifierName)
        {
            Modifier modifier = m_Modifiers.Find(m => m.Name == modifierName);
            return modifier != null;
        }

        public Coroutine ScheduleCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void UnScheduleCoroutine(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
}