using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

namespace UAS
{
    
    public interface IAbility
    {
        void UseAbility();
        void UseAbilityOnTarget(IUnit target);
        void UseAbilityOnPosition(Vector3 position);
        bool IsBehaviorNoTarget();
        bool IsBehaviorTarget();
        bool IsBehaviorPoint();
        bool IsPassive();
        
        Modifier CreateModifier(string modifierName,float duration, IUnit caster, IUnit owner);
    }
    public class Ability :IAbility
    {
        private static readonly string s_OnAbilityStartedKey = "OnAbilityStarted";
        private static readonly string s_OnAbilityExecutedKey = "OnAbilityExecuted";
        
        protected AbilityData m_Data;
        protected AbilityTargetBehavior m_TargetBehavior;
        protected Dictionary<string, List<Effect>> m_EventActions = new();
        protected IUnit m_Caster;
        protected IUnit m_Target;
        protected Vector3? m_TargetPoint;
        public Ability(AbilityData data, IUnit caster)
        {
            m_Caster = caster;
            m_Data = data;
            m_TargetBehavior = data.targetBehavior;
            ProcessEventOnData(data.events);
            ProcessModifierData(data.modifiers);
        }
        protected void ProcessEventOnData(List<EventOnData> eventOnsData)
        {
            foreach (var eventOnData in eventOnsData)
            {
                if(eventOnData.effects.Count == 0)
                    continue;
                
                var effects = new List<Effect>();
                m_EventActions.Add(eventOnData.eventName, effects);
                foreach (var actionData in eventOnData.effects)
                {
                    Effect effect = EffectFactory.Create(actionData);
                    effects.Add(effect);
                }
            }
        }

        protected void ProcessModifierData(List<ModifierData> modifiersData)
        {
            foreach (var modifierData in modifiersData)
            {
                if (modifierData.passive)
                {
                    m_Caster.AddNewModifier(modifierData.name, 0f, m_Caster, this);
                }
            }
        }

        
        public void UseAbility()
        {
            ExectueAbilityTasks().Forget();
        }

        public void UseAbilityOnTarget(IUnit target)
        {
            m_Target = target;
            ExectueAbilityTasks().Forget();
        }

        public void UseAbilityOnPosition(Vector3 position)
        {
            m_TargetPoint = position;
            ExectueAbilityTasks().Forget();
        }

        public bool IsBehaviorNoTarget()
        {
            return m_TargetBehavior == AbilityTargetBehavior.NoTarget;
        }

        public bool IsBehaviorTarget()
        {
            return m_TargetBehavior == AbilityTargetBehavior.Target;
        }

        public bool IsBehaviorPoint()
        {
            return m_TargetBehavior == AbilityTargetBehavior.Point;
        }

        public bool IsPassive()
        {
            return m_TargetBehavior == AbilityTargetBehavior.Passive;
        }

        public Modifier CreateModifier(string modifierName, float duration, IUnit caster, IUnit owner)
        {
            ModifierData modifierData = m_Data.modifiers.Find(data => data.name == modifierName);
            if (modifierData != null)
            {
                var modifier = new Modifier(modifierData,duration,  owner,  caster, this);
                return modifier;
            }
            else
            {
                Debug.LogError("No modifier data in ability: " + modifierName);
                return null;
            }
        }

        
        protected virtual async UniTask ExectueAbilityTasks()
        {
            try
            {
                await StartAbility();
                ExecuteAbility();
            }
            catch (OperationCanceledException e)
            {
                Debug.Log("Ability Canceled");
            }
        }
        
        protected virtual async UniTask StartAbility()
        {
            await UniTask.CompletedTask;
            ExecuteEventActions(s_OnAbilityStartedKey);
            OnAbilityStarted();
        }

        protected virtual void ExecuteAbility()
        {
            ExecuteEventActions(s_OnAbilityExecutedKey);
            OnAbilityExecuted();
        }
        
        protected void ExecuteEventActions(string eventKey)
        {
            if (m_EventActions.TryGetValue(eventKey, out var effects))
            {
                EffectParams effectParams = new EffectParams
                {
                    ability = this,
                    caster =  m_Caster,
                    target =  m_Target,
                    point =  m_TargetPoint,
                };
                
                foreach (var effect in effects)
                {
                    effect.Execute(ref effectParams);
                }
            }
        }

        protected virtual void OnAbilityStarted()
        {
            
        }

        protected virtual void OnAbilityExecuted()
        {
            
        }
    }
}