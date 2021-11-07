using PabloLario.Characters.Core.Stats;
using UnityEngine;
using PabloLario.Characters.Enemies;
using PabloLario.Animations;
using Assets.Scripts.Characters.Boss;

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
    public float timeInEnrageBeforeStartMoving;

    [Header("Hit Animation")]
    public HitColorChangeAnimation hitAnimation;

    [Header("Boss UI Controller")]
    public BossUI bossUI;

    private void Start()
    {
        currentHealth = maxHealth;
        bossUI.SetSlider(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        StartCoroutine(hitAnimation.Co_HitColorChange());
        bossUI.UpdateSliderValue(currentHealth);

        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else if (currentHealth - damage <= Mathf.FloorToInt(maxHealth / 2f))
        {
            Enrage();
        }
    }

    private void Enrage()
    {
        moveSpeed = 4f;
        timeBetweenBulletsInBurstShooting = 0.04f;
        bulletStats.Speed = 10f;
        timeBetweenWavesWhenMoving = 0.85f;
        timeBetweenWavesWhenStopped = 0.4f;
        numberOfBulletsInWavesWhenMoving = 30;
        numberOfBulletsInWavesWhenStopped = 40;
        timeBetweenBursts = 1.10f;

        OnBossEnrage?.Invoke();
    }

    private void Die()
    {
        bossUI.DisableSlider();
        OnBossDeath?.Invoke();
    }
}
