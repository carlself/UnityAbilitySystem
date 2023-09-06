using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class HealthBar : MonoBehaviour
    {
        public RPGUnit unit;
        public Slider slider;
        public TMP_Text text;

        private void Start()
        {
            UpdateHealth();
            unit.StatContainer.onStatUpdated += (stat) =>
            {
                if (stat is Health || stat is MaxHealth)
                {
                    UpdateHealth();
                }
            };
        }

        private void UpdateHealth()
        {
            float health = unit.StatContainer.GetStatValue<Health>();
            float maxHealth = unit.StatContainer.GetStatValue<MaxHealth>();
            slider.value = health / maxHealth;
            text.text = $"{health} / {maxHealth}";
        }
    }
}