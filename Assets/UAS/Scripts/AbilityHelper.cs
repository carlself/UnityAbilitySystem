using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace UAS
{
    public static class AbilityHelper
    {
        private static Dictionary<string, Type> m_EventTypeRegistry = new();
        static AbilityHelper()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(t => typeof(IModifierEvent).IsAssignableFrom(t));
            foreach (Type type in types)
            {
                m_EventTypeRegistry.Add(type.Name, type);
            }
        }
        
        public static Type ModifierEventNameToType(string eventName)
        {
            if (m_EventTypeRegistry.TryGetValue(eventName, out var type))
            {
                return type;
            }

            return null;
        }
    }
}