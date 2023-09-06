using System;
using System.Collections;
using System.Collections.Generic;
using CodiceApp.EventTracking.Plastic;
using JetBrains.Annotations;
using UAS.Stats;
using UnityEngine;

namespace UAS
{
    
    public class Modifier
    {
        private ModifierData m_Data;
        private EventBus<IModifierEvent> m_EventBus = new();
        private Dictionary<Type, List<Effect>> m_EventActions = new();
        private Dictionary<string, StatModifierData> m_StatModifierDict = new();
        public Dictionary<string, StatModifierData> StatModifierDict => m_StatModifierDict;
        
        private Dictionary<string, StateModifierData> m_StatusModifierDict = new();
        public Dictionary<string, StateModifierData> StateModifierDict => m_StatusModifierDict;

        private string m_Name;
        private float m_Duration;
        public string Name => m_Name;
        [CanBeNull] private IAbility m_Ability;
        [CanBeNull] private IUnit m_Caster;
        private IUnit m_Owner;
        private Coroutine m_UpdateScheduler;
        private Coroutine m_ExpireScheduler;
        public Modifier(ModifierData data, float duration, IUnit owner, IUnit caster = null, IAbility ability = null)
        {
            m_Data = data;
            m_Owner = owner;
            m_Ability = ability;
            m_Caster = caster;
            m_Name = data.name;

            if (m_Data.duration > 0f)
                m_Duration = duration;
            if (duration > 0f)
                m_Duration = duration;
            if (data.events != null)
            {
                foreach (var eventOnData in data.events)
                {
                    Type eventType = AbilityHelper.ModifierEventNameToType(eventOnData.eventName);
                    if (eventType == null)
                    {
                        Debug.LogError("Invalid eventName " + eventOnData.eventName);
                        continue;
                    }

                    m_EventBus.Subscribe(eventType, OnModifierEvent);

                    var actions = new List<Effect>();
                    m_EventActions.Add(eventType, actions);
                    foreach (var actionData in eventOnData.effects)
                    {
                        Effect effect = EffectFactory.Create(actionData);
                        actions.Add(effect);
                    }
                }
            }

            ProcessStatModifiersData(data.stats);
            ProcessStateModifiersData(data.states);

            if (data.updateInterval > 0f)
            {
                m_UpdateScheduler = owner.ScheduleCoroutine(UpdateModifier());
            }
            
            
            if (m_Duration > 0f)
            {
                m_ExpireScheduler = owner.ScheduleCoroutine(ExpireModifier());
            }
        }

        private void ProcessStatModifiersData(List<StatModifierData> statModifiersData)
        {
            if(statModifiersData == null)
                return;

            foreach (var modiferData in statModifiersData)
            {
                if (m_StatModifierDict.ContainsKey(modiferData.statName))
                {
                    Debug.LogError("Duplicate modifier stat: " + modiferData.statName);
                    continue;
                }
                m_StatModifierDict.Add(modiferData.statName, modiferData);
            }
        }

        private void ProcessStateModifiersData(List<StateModifierData> stateModifiersData)
        {
            if(stateModifiersData == null)
                return;

            foreach (var stateModifierData in stateModifiersData)
            {
                if (m_StatusModifierDict.ContainsKey(stateModifierData.stateName))
                {
                    Debug.LogError("Duplicate modifier status: "+stateModifierData.stateName);
                    continue;
                }
                m_StatusModifierDict.Add(stateModifierData.stateName, stateModifierData);
            }
        }

        public void Destroy()
        {
            if (m_ExpireScheduler != null)
            {
                m_Owner.UnScheduleCoroutine(m_ExpireScheduler);
                m_ExpireScheduler = null;
            }

            if (m_UpdateScheduler != null)
            {
                m_Owner.UnScheduleCoroutine(m_UpdateScheduler);
                m_UpdateScheduler = null;
            }
            
            OnDestroy();
        }

        protected virtual void OnDestroy()
        {
            
        }

        private IEnumerator UpdateModifier()
        {
            while (true)
            {
                OnUpdate onUpdate = new();
                RaiseEvent(ref onUpdate);
                yield return new WaitForSeconds(m_Data.updateInterval);
            }
        }

        private IEnumerator ExpireModifier()
        {
            yield return new WaitForSeconds(m_Duration);
            m_Owner.RemoveModifier(this);
        }

        private void OnModifierEvent(ref IModifierEvent eventData)
        {
            Type eventType = eventData.GetType();

            if (m_EventActions.TryGetValue(eventType, out var actions))
            {
                EffectParams effectParams = new EffectParams
                {
                    modifier = this,
                    ability = m_Ability,
                    caster =  m_Caster,
                };
                eventData.FillActionParams(ref effectParams);

                foreach (var action in actions)
                {
                    action.Execute(ref effectParams);
                }
            }
        }
        
        public void RaiseEvent<TEvent>(ref TEvent eventData) where TEvent:IModifierEvent
        {
            m_EventBus.Raise(ref eventData);
        }
    }
}