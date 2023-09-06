using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UAS;
using UAS.Tests;
using UnityEngine;

public class StateTests : MonoBehaviour
{
    private StateContainer<TestState> m_StateContainer;
    [SetUp]
    public void Setup()
    {
        m_StateContainer = new StateContainer<TestState>();
    }

    [TestCase(TestState.State1)]
    [TestCase(TestState.State2)]
    public void InitNoState(TestState state)
    {
        Assert.That(!m_StateContainer.HasState(state));
    }

    [TestCase("State1")]
    [TestCase("State2")]
    public void ModifyState(string stateName)
    {
        var modifierData = new ModifierData()
        {
            states = new List<StateModifierData>
            {
                new ()
                {
                    stateName = stateName
                }
            }
        };
        var modifier = new Modifier(modifierData, 0, null);
        m_StateContainer.AddModifier(modifier);
        Enum.TryParse(stateName, out TestState state);
        
        Assert.That(m_StateContainer.HasState(state));
    }
}
