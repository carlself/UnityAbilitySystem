using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UAS;
using UAS.Tests;
using UnityEngine;
using UnityEngine.TestTools;

public class ModifierTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void ModifierEventTriggerEffect()
    {
        var modifierData = new ModifierData
        {
            events = new List<EventOnData>()
            {
                new()
                {
                    eventName =  "TestModifierEvent",
                    effects =  new List<EffectData>()
                    {
                        new TestEffectData()
                        {
                            name =  "TestEffect"
                        }
                    }
                }
            }
        };
        Modifier modifier = new Modifier(modifierData, 0, null);
        IModifierEvent modifierEvent = new TestModifierEvent();
        modifier.RaiseEvent(ref modifierEvent);

        TestEffect effect = modifier.GetModifierEffect(typeof(TestModifierEvent)) as TestEffect;
        
        Assert.IsNotNull(effect);
        Assert.That(effect.value == 1);
    }
    
    [UnityTest]
    public IEnumerator ModiferDuration()
    {
        var owner = new GameObject("Unit").AddComponent<TestUnit>();
        Modifier modifier = new Modifier(new ModifierData(), 1f, owner);
        owner.AddModifier(modifier);
        
        Assert.IsTrue(owner.HasModifier(modifier.Name));
        
        yield return new WaitForSeconds(1.1f);
        
        Assert.IsTrue(!owner.HasModifier(modifier.Name));
    }

    [UnityTest]
    public IEnumerator ModifierTick()
    {
        var modifierData = new ModifierData
        {
            updateInterval =  1,
            events = new List<EventOnData>()
            {
                new()
                {
                    eventName =  "OnUpdate",
                    effects =  new List<EffectData>()
                    {
                        new TestEffectData()
                        {
                            name =  "TestEffect"
                        }
                    }
                }
            }
        };
        
        var owner = new GameObject("Unit").AddComponent<TestUnit>();
        Modifier modifier = new Modifier(modifierData, 3f, owner);
        owner.AddModifier(modifier);
        
        yield return new WaitForSeconds(3f);
        
        TestEffect effect = modifier.GetModifierEffect(typeof(OnUpdate)) as TestEffect;
        
        Assert.IsNotNull(effect);
        Assert.That(effect.value == 3);
    }
}
