using System;
using System.Collections.Generic;
using UnityEngine;

namespace UAS.Stats
{
    public delegate void OnStatUpdated(Stat stat);
    public class StatContainer
    {
        
        
        private Dictionary<Type, Stat> m_StatTypeDict = new();
        private Dictionary<Type, List<StatModifier>> m_StatModifierDict = new();
        public OnStatUpdated onStatUpdated;
        
        public void AddStat(Stat stat)
        {
            Type statType = stat.GetType();
            if (m_StatTypeDict.ContainsKey(statType))
            {
                Debug.LogError("Duplicate Stat: " + statType.Name);
            }
            m_StatTypeDict.Add(statType, stat);
        }

        public bool HasStat<T>() where T : Stat
        {
            return m_StatTypeDict.ContainsKey(typeof(T));
        }
        
        public float GetStatValue<T>() where T : Stat
        {
            if (m_StatTypeDict.TryGetValue(typeof(T), out Stat stat))
            {
                return stat.value;
            }

            return 0f;
        }

        public void SetStatValue<T>(float value) where T : Stat
        {
            if (m_StatTypeDict.TryGetValue(typeof(T), out Stat stat))
            {
                stat.value = value;
                onStatUpdated?.Invoke(stat);
            }
            else
            {
                Debug.LogError("Stat not found: "+typeof(T).Name);
            }
        }

        public void ChangeStatValue<T>(float amount) where T : Stat
        {
            if (m_StatTypeDict.TryGetValue(typeof(T), out Stat stat))
            {
                stat.value += amount;
                onStatUpdated?.Invoke(stat);
            }
            else
            {
                Debug.LogError("Stat not found: "+typeof(T).Name);
            }
        }

        protected virtual void UpdateStatValue(Stat stat)
        {
            if(!m_StatModifierDict.TryGetValue(stat.GetType(), out var statModifiers) 
               || statModifiers.Count == 0)
            {
                stat.value = stat.baseValue;
                onStatUpdated?.Invoke(stat);
                return;
            }
            
            float bonus = 0f;
            float percent = 1f;
            bool hasConst = false;
            foreach (var statModifier in statModifiers)
            {
                switch (statModifier.op)
                {
                    case ModifyOp.Bonus:
                        bonus += statModifier.value;
                        break;
                    case ModifyOp.Constant:
                        hasConst = true;
                        bonus = statModifier.value;
                        break;
                    case ModifyOp.Percent:
                        percent *= statModifier.value / 100f;
                        break;
                    case ModifyOp.BonusPercent:
                        percent *= (statModifier.value + 100f) / 100f;
                        break;
                }
            }

            if (hasConst)
            {
                stat.value = bonus;
            }
            else
            {
                stat.value = (stat.baseValue + bonus) *percent;
            }
            
            onStatUpdated?.Invoke(stat);
        }

        public void AddModifier(Modifier modifier)
        {
            if(modifier.StatModifierDict == null)
                return;
            foreach (var entry in modifier.StatModifierDict)
            {
                Stat stat = GetStatByName(entry.Key);
                if (stat == null)
                {
                    Debug.LogError("Stat not found: "+entry.Key);
                    continue;
                }

                if (!m_StatModifierDict.TryGetValue(stat.GetType(), out var statModifiers))
                {
                    statModifiers = new List<StatModifier>();
                    m_StatModifierDict.Add(stat.GetType(), statModifiers);
                }
                
                statModifiers.Add(new StatModifier(modifier, entry.Value.value, entry.Value.op));
                UpdateStatValue(stat);
            }
        }
        
        public void RemoveModifier(Modifier modifier)
        {
            if(modifier.StatModifierDict == null)
                return;
            
            foreach (var entry in modifier.StatModifierDict)
            {
                Stat stat = GetStatByName(entry.Key);
                if (stat == null)
                {
                    Debug.LogError("Stat not found: "+entry.Key);
                    continue;
                }

                if (!m_StatModifierDict.TryGetValue(stat.GetType(), out var statModifiers))
                {
                    return;
                }

                statModifiers.RemoveAll(statModifier => statModifier.modifier == modifier);
                UpdateStatValue(stat);
            }
        }

        public Stat GetStatByName(string name)
        {
            foreach (var entry in m_StatTypeDict)
            {
                if (entry.Value.GetName().Equals(name, StringComparison.Ordinal))
                    return entry.Value;
            }

            return null;
        }
    }
}