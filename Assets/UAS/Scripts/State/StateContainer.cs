using System;
using System.Collections.Generic;
using UAS.Stats;
using UnityEngine;

namespace UAS
{
    public delegate void OnStateUpdate<TStateFlag>(TStateFlag state, bool isOn) where TStateFlag: unmanaged, Enum;
    public class StateContainer<TStateFlag> where TStateFlag: unmanaged, Enum
    {
        
        private TStateFlag m_State;
        private Dictionary<TStateFlag, List<Modifier>>  m_StateModifierDict = new ();
        public OnStateUpdate<TStateFlag> onStateUpdated;
        public bool HasState(TStateFlag state)
        {
            return m_State.HasFlag(state);
        }
        
        protected virtual void UpdateState(TStateFlag state)
        {
            m_State = m_State.ClearFlags(state);
            if (m_StateModifierDict.TryGetValue(state, out var modifiers) && modifiers.Count > 0)
            {
                m_State = m_State.SetFlags(state);
            }
            onStateUpdated?.Invoke(state, HasState(state));
        }
        
        public void AddModifier(Modifier modifier)
        {
            if(modifier.StateModifierDict == null)
                return;
            
            foreach (var entry in modifier.StateModifierDict)
            {
                if (!Enum.TryParse(entry.Key, out TStateFlag state))
                {
                    Debug.LogError("Invalid State: "+entry.Key);
                    continue;
                }

                if (!m_StateModifierDict.TryGetValue(state, out var modifiers))
                {
                    modifiers = new List<Modifier>();
                    m_StateModifierDict.Add(state, modifiers);
                }
                modifiers.Add(modifier);
                UpdateState(state);
            }
        }

        public void RemoveModifier(Modifier modifier)
        {
            if(modifier.StateModifierDict == null)
                return;
            foreach (var entry in modifier.StateModifierDict)
            {
                if (!Enum.TryParse(entry.Key, out TStateFlag state))
                {
                    Debug.LogError("Invalid State: "+entry.Key);
                    continue;
                }
                
                if (m_StateModifierDict.TryGetValue(state, out var modifiers))
                {
                    modifiers.Remove(modifier);
                }
                UpdateState(state);
            }
        }
    }
}