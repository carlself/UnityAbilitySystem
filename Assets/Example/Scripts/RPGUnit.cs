using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UAS;
using UAS.Stats;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[Flags]
public enum UnitState
{
    None = 0,
    Stun = 1 << 0,
}

namespace Example
{
    public class RPGUnit : Unit<UnitState>
    {
        public AbilityData[] abilitiesData;
        public float health;
        public float maxHealth;
        public Transform headTransform;

        private List<Ability> m_Abilities = new();
        private AbilitySelectTarget m_SelectTarget;

        // Start is called before the first frame update
        void Awake()
        {
            m_SelectTarget = GetComponent<AbilitySelectTarget>();
            foreach (var abilityData in abilitiesData)
            {
                var ability = new Ability(abilityData, this);
                m_Abilities.Add(ability);
            }

            Health healthStat = new Health(health);
            m_StatContainer.AddStat(healthStat);
            MaxHealth maxHealthStat = new MaxHealth(maxHealth);
            m_StatContainer.AddStat(maxHealthStat);
            m_StateContainer.onStateUpdated += (state, isOn) =>
            {
                if (state == UnitState.Stun)
                {
                    Debug.Log("Stun " + (isOn ? "begin":"end"));
                }
            };
        }

        public void UseAbility(int index)
        {
            if (index < 0 && index >= m_Abilities.Count)
                return;

            Ability ability = m_Abilities[index];
            if (ability.IsPassive())
            {
                Debug.LogError("Can't use passive ability");
            }
            else if (ability.IsBehaviorNoTarget())
            {
                ability.UseAbility();
            }
            else if (ability.IsBehaviorTarget())
            {
                m_SelectTarget.StartSelectingUnit(target =>
                {
                    if (target != null)
                    {
                        ability.UseAbilityOnTarget(target);
                    }
                });
            }
            else if(ability.IsBehaviorPoint())
            {
                m_SelectTarget.StartSelectingPoint(point =>
                {
                    if (point != null)
                    {
                        ability.UseAbilityOnPosition(point.Value);
                    }
                });
            }
        }

        public void Heal(float amount)
        {
            float health = m_StatContainer.GetStatValue<Health>();
            float maxHeath = m_StatContainer.GetStatValue<MaxHealth>();
            health = Mathf.Clamp(health + amount, 0, maxHeath);
            m_StatContainer.SetStatValue<Health>(health);
            Debug.Log("health " + health);
        }

        public void TakeDamage(float damage)
        {
            float health = m_StatContainer.GetStatValue<Health>();
            health = health - damage;
            m_StatContainer.SetStatValue<Health>(health);
            Debug.Log("health " + health);
        }
    }
}
