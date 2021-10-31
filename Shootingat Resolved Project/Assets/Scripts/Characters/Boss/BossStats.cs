using PabloLario.Characters.Core.Stats;
using UnityEngine;
using PabloLario.Characters.Enemies;
using PabloLario.Animations;

public class BossStats : MonoBehaviour, IDamageable
{
    public delegate void BossChangingState();
    public static event BossChangingState OnBossDeath;
    public static event BossChangingState OnBossEnrage;

    [Header("Health")]
    public int maxHealth;
    private int currentHealth;

    [Header("Move Speed")]
    public float moveSpeed;

    [Header("Bullet Stats")]
    public EnemyBulletStats bulletStats;

    [Header("Shooting Stats")]
    public float timeBetweenWavesWhenMoving;
    public float timeBetweenWavesWhenStopped;
    public float fireRateWhenStoppedAndTargettingPlayer;
    public int numberOfBulletsInWavesWhenMoving;
    public int numberOfBulletsInWavesWhenStopped;
    public float timeInStopTillStartShooting;
    public float timeBetweenBulletsInBurstShooting;
    public float timeBetweenBursts;

    [Header("Hit Animation")]
    public HitColorChangeAnimation hitAnimation;

    public void TakeDamage(int damage)
    {
        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else if (currentHealth - damage <= Mathf.FloorToInt(maxHealth / 2f))
        {
            currentHealth -= damage;
            Enrage();
        }
    }

    private void Enrage()
    {
        // Here I need to change all the variables to the enrage phase

        OnBossEnrage?.Invoke();
    }

    private void Die()
    {
        OnBossDeath?.Invoke();
    }
}
