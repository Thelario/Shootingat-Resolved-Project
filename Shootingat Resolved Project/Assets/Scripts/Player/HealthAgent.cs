using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthAgent : MonoBehaviour, IDamageable
{

    [Header("Fields")]
    [SerializeField] protected Color agentColor;
    [SerializeField] protected Color hitColor;
    [SerializeField] protected Color healthSliderColor;
    [SerializeField] protected bool showHealthSlider;
    [SerializeField] protected int maxHealth;
    protected int currentHealth;

    [Header("References")]
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected Image fillImage;
    [SerializeField] protected SpriteRenderer agentRenderer;

     protected virtual void Start()
    {
        healthSlider.gameObject.SetActive(showHealthSlider);
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        healthSlider.minValue = 0f;
        fillImage.color = healthSliderColor;
        agentRenderer.color = agentColor;
    }

    /// <summary>
    /// Deal damage to the agent that has been hit
    /// </summary>
    /// <param name="damage"> Amount of damage to deal to the agent that has been hit </param>
    public virtual void TakeDamage(int damage)
    {
        // Enable the healthSlider if not enabled before
        if (healthSlider.IsActive() == false)
            healthSlider.gameObject.SetActive(showHealthSlider);

        currentHealth -= damage;

        UpdateSlider();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void HealHealth(int health)
    {
        currentHealth += health;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;

        UpdateSlider();
    }

    /// <summary>
    /// Updates slider's UI when health changes
    /// </summary>
    private void UpdateSlider()
    {
        healthSlider.value = currentHealth;
        StartCoroutine(HitColorChange());
    }

    /// <summary>
    /// Function used to blink and change the colors of the enemy and slider when an enemy is hitted by 
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitColorChange()
    {
        agentRenderer.color = hitColor;

        yield return new WaitForSeconds(0.05f);

        agentRenderer.color = agentColor;
    }

    public abstract void Die();
}
