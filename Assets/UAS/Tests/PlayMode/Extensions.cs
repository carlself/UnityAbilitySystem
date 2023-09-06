using System;
using System.Collections.Generic;
using System.Reflection;

namespace UAS.Tests
{
    public static class Extensions
    {
        public static Effect GetModifierEffect(this Modifier modifier, Type eventType)
        {
            FieldInfo fieldInfo = typeof(Modifier).GetField("m_EventActions", BindingFlags.NonPublic | BindingFlags.Instance);
            var effectDict = fieldInfo.GetValue(modifier) as Dictionary<Type, List<Effect>>;
            if (effectDict.TryGetValue(eventType, out var effects))
            {
                return effects[0];
            }

            return null;
        }
    }
}