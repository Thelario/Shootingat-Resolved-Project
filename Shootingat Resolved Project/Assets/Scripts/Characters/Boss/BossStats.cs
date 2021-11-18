using System.Collections;
using PabloLario.Characters.Core.Stats;
using UnityEngine;
using PabloLario.Characters.Enemies;
using PabloLario.Animations;
using Assets.Scripts.Characters.Boss;
using UnityEngine.Serialization;

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

    public bool enraging = false;
    private bool _hasEnraged = false;
    private bool _invencible = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
        bossUI.SetSlider(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (enraging || _invencible)
            return;
        
        currentHealth -= damage;

        StartCoroutine(hitAnimation.Co_HitColorChange());
        bossUI.UpdateSliderValue(currentHealth);

        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else if (currentHealth - damage <= Mathf.FloorToInt(maxHealth / 2f) && !_hasEnraged)
        {
            Enrage();
        }
    }

    public void MakeBossInvencible(bool invencible)
    {
        _invencible = invencible;
    }

    private void Enrage()
    {
        _hasEnraged = true;
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
