using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UAS
{
    public class EventBus<TBaseEvent>
    {
        public delegate void EventHandler(ref TBaseEvent eventData);
        
        private Dictionary<Type, EventHandler> m_Handlers = new();

        public void Raise<TEvent>(TEvent eventData) where TEvent : TBaseEvent
        {
            Raise(ref eventData);
        }
        
        public void Raise<TEvent>(ref TEvent eventData) where TEvent: TBaseEvent
        {
            Type eventType = eventData.GetType();
            try
            {
                if (m_Handlers.TryGetValue(eventType, out EventHandler h))
                {
                    // boxing here
                    TBaseEvent baseEvent = eventData;
                    h.Invoke(ref baseEvent);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public void Subscribe(Type eventType, EventHandler handler)
        {
            if (m_Handlers.TryGetValue(eventType, out EventHandler h))
            {
                h += handler;
                m_Handlers[eventType] = h;
            }
            else
            {
                m_Handlers[eventType] = handler;
            }
        }
        
        public void Unsubscribe(Type eventType, EventHandler handler)
        {
            if (m_Handlers.TryGetValue(eventType, out EventHandler h))
            {
                h -= handler;
                if (h != null) m_Handlers[eventType] = h;
                else m_Handlers.Remove(eventType);
            }
        }
    }
}