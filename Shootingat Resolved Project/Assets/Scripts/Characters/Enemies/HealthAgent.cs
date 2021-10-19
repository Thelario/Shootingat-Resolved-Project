using PabloLario.Animations;
using PabloLario.Characters.Core.Stats;
using System.Collections;
using UnityEngine;

namespace PabloLario.Characters.Enemies
{
    public abstract class HealthAgent : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int maxHealth;

        public HitColorChangeAnimation hitAnimation;

        protected int currentHealth;

        protected virtual void Start()
        {
            currentHealth = maxHealth;
        }

        public virtual void TakeDamage(int damage)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
            else
            {
                StartCoroutine(hitAnimation.Co_HitColorChange());
            }
        }

        public virtual void HealHealth(int health)
        {
            currentHealth += health;

            if (currentHealth >= maxHealth)
                currentHealth = maxHealth;
        }

        public abstract void Die();
    }
}
