using PabloLario.Shooting;
using System.Collections;
using UnityEngine;

namespace PabloLario.Enemies
{
    public abstract class HealthAgent : MonoBehaviour, IDamageable
    {
        [Header("Fields")]
        [SerializeField] protected Color agentColor;
        [SerializeField] protected Color hitColor;
        [SerializeField] protected int maxHealth;
        [SerializeField] protected float timeToWaitForColorChange;

        [Header("References")]
        [SerializeField] protected SpriteRenderer agentRenderer;

        protected int currentHealth;

        protected virtual void Start()
        {
            currentHealth = maxHealth;
            agentRenderer.color = agentColor;
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
                StartCoroutine(Co_HitColorChange());
            }
        }

        public virtual void HealHealth(int health)
        {
            currentHealth += health;

            if (currentHealth >= maxHealth)
                currentHealth = maxHealth;
        }

        private IEnumerator Co_HitColorChange()
        {
            agentRenderer.color = hitColor;

            yield return new WaitForSeconds(timeToWaitForColorChange);

            agentRenderer.color = agentColor;
        }

        public abstract void Die();
    }
}
