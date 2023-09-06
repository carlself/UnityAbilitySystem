using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace UAS
{
    public static class EffectFactory
    {
        private static Dictionary<string, Type> m_Registry = new();

        static EffectFactory()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(t => typeof(Effect).IsAssignableFrom(t));

            foreach (var t in types)
            {
                m_Registry.Add(t.Name, t);
            }
        }
        public static Effect Create(EffectData effectData)
        {
            if (m_Registry.TryGetValue(effectData.name, out var type))
            {
                Effect effect = Activator.CreateInstance(type, new object[] { effectData }) as Effect;
                return effect;
            }
            return null;
        }
    }
}