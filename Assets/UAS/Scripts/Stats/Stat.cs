using System;
using UnityEngine;

namespace UAS
{
    public struct StatModifier
    {
        public Modifier modifier;
        public float value;
        public ModifyOp op;

        public StatModifier(Modifier modifier, float value, ModifyOp op)
        {
            this.modifier = modifier;
            this.value = value;
            this.op = op;
        }
    }

    public abstract class Stat
    {
        public float baseValue;
        public float value;
        
        public Stat(float value)
        {
            baseValue = value;
            this.value = value;
        }
        
        public abstract string GetName();
    }
    
    
}