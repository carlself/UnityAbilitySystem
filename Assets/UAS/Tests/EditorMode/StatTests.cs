using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UAS;
using UAS.Stats;
using UAS.Tests;

public class StatTests
{
    private StatContainer m_StatContainer;
    [SetUp]
    public void Setup()
    {
        m_StatContainer = new StatContainer();
        m_StatContainer.AddStat(new TestStat(100f));
    }
    [TestCase(0)]
    [TestCase(100)]
    public void SetStat(float value)
    {
        m_StatContainer.SetStatValue<TestStat>(value);
        Assert.That(m_StatContainer.GetStatValue<TestStat>(), Is.EqualTo(value).Within(float.Epsilon));
    }

    [TestCase(0)]
    [TestCase(200)]
    public void ModifyStatByBonus(float bonus)
    {
        var modifierData = new ModifierData()
        {
            stats = new List<StatModifierData>
            {
                new ()
                {
                    statName = "TestStat",
                    value = bonus,
                    op = ModifyOp.Bonus
                }
            }
        };
        var modifier = new Modifier(modifierData, 0, null);
        m_StatContainer.AddModifier(modifier);
        
        Assert.That(m_StatContainer.GetStatValue<TestStat>(), Is.EqualTo(100f + bonus).Within(float.Epsilon));
    }
    
    [TestCase(0)]
    [TestCase(50)]
    [TestCase(100)]
    public void ModifyStatByBercent(float percent)
    {
        var modifierData = new ModifierData()
        {
            stats = new List<StatModifierData>
            {
                new ()
                {
                    statName = "TestStat",
                    value = percent,
                    op = ModifyOp.Percent
                }
            }
        };
        var modifier = new Modifier(modifierData, 0, null);
        m_StatContainer.AddModifier(modifier);
        
        Assert.That(m_StatContainer.GetStatValue<TestStat>(), Is.EqualTo(100f * percent/100f).Within(float.Epsilon));
    }
    
    [TestCase(0)]
    [TestCase(50)]
    [TestCase(100)]
    public void ModifyStatByBonusPercent(float percent)
    {
        var modifierData = new ModifierData()
        {
            stats = new List<StatModifierData>
            {
                new ()
                {
                    statName = "TestStat",
                    value = percent,
                    op = ModifyOp.BonusPercent
                }
            }
        };
        var modifier = new Modifier(modifierData, 0, null);
        m_StatContainer.AddModifier(modifier);
        
        Assert.That(m_StatContainer.GetStatValue<TestStat>(), Is.EqualTo(100f * (100f + percent)/100f).Within(float.Epsilon));
    }
    
    [TestCase(0)]
    [TestCase(50)]
    [TestCase(100)]
    public void ModifyStatByConst(float value)
    {
        var modifierData = new ModifierData()
        {
            stats = new List<StatModifierData>
            {
                new ()
                {
                    statName = "TestStat",
                    value = value,
                    op = ModifyOp.Constant
                }
            }
        };
        var modifier = new Modifier(modifierData, 0, null);
        m_StatContainer.AddModifier(modifier);
        
        Assert.That(m_StatContainer.GetStatValue<TestStat>(), Is.EqualTo(value).Within(float.Epsilon));
    }
    
    [Test]
    public void ModifyStatByThreeModifiers()
    {
        var modifierData1 = new ModifierData()
        {
            stats = new List<StatModifierData>
            {
                new ()
                {
                    statName = "TestStat",
                    value = 10,
                    op = ModifyOp.Bonus
                },
            }
        };
        var modifierData2 = new ModifierData()
        {
            stats = new List<StatModifierData>
            {
                new ()
                {
                    statName = "TestStat",
                    value = 20,
                    op = ModifyOp.BonusPercent
                }
            }
        };
        var modifierData3 = new ModifierData()
        {
            stats = new List<StatModifierData>
            {
                new ()
                {
                    statName = "TestStat",
                    value = 30,
                    op = ModifyOp.Percent
                }
            }
        };
        var modifier1 = new Modifier(modifierData1, 0, null);
        m_StatContainer.AddModifier(modifier1);
        
        var modifier2 = new Modifier(modifierData2, 0, null);
        m_StatContainer.AddModifier(modifier2);
        
        var modifier3 = new Modifier(modifierData3, 0, null);
        m_StatContainer.AddModifier(modifier3);
        
        Assert.That(m_StatContainer.GetStatValue<TestStat>(), Is.EqualTo((100f + 10f) * ((100f + 20f)/100f) * (30f/100f)) .Within(float.Epsilon));
    }

    [Test]
    public void ModifyByRemoveModifier()
    {
        var modifierData = new ModifierData()
        {
            stats = new List<StatModifierData>
            {
                new ()
                {
                    statName = "TestStat",
                    value = 100f,
                    op = ModifyOp.Bonus
                }
            }
        };
        var modifier = new Modifier(modifierData, 0, null);
        m_StatContainer.AddModifier(modifier);
        
        Assert.That(m_StatContainer.GetStatValue<TestStat>(), Is.EqualTo(200f).Within(float.Epsilon));
        
        m_StatContainer.RemoveModifier(modifier);
        Assert.That(m_StatContainer.GetStatValue<TestStat>(), Is.EqualTo(100f).Within(float.Epsilon));
    }
    
}
