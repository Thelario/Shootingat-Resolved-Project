using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable CS0114

public class PlayerStats : Singleton<PlayerStats>, IDamageable
{
    [Header("Player Stats")]

    [Header("Clarity")]
    public int currentClarity;
    [SerializeField] private int maxCurrentClarity;
    [SerializeField] private int lastMaxClarity;

    [Header("Bullet Speed")]
    public float bulletSpeed;
    [SerializeField] private float maxBulletSpeed;

    [Header("Fire Rate")]
    public float fireRate;
    [SerializeField] private float minFireRate;

    [Header("Bullet Damage")]
    public int bulletDamage;
    [SerializeField] private int maxBulletDamage;

    [Header("Bullet Range")]
    public float bulletRange;
    [SerializeField] private float maxBulletRange;

    [Header("Player Move Speed")]
    public float moveSpeed;
    [SerializeField] private float maxMoveSpeed;

    [Header("References")]
    [SerializeField] private PlayerStatsUI psui;
    [SerializeField] private PlayerClarity pc;

    private void Start()
    {
        psui.ModifyDamageText(bulletDamage);
        psui.ModifyFireRateText(fireRate);
        psui.ModifyHealthText(currentClarity, maxCurrentClarity);
        psui.ModifyPlayerSpeedText(moveSpeed);
        psui.ModifyBulletSpeedText(bulletSpeed);
        psui.ModifyBulletRangeText(bulletRange);
        pc.UpdateClarity(currentClarity, maxCurrentClarity);
    }

    /// <summary>
    /// All necessary actions to happen when an enemy dies
    /// </summary>
    public void Die()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Deals x amount of damage to player
    /// </summary>
    /// <param name="damage"> Amount of damage to deal </param>
    public void TakeDamage(int damage)
    {
        currentClarity -= damage;
        if (currentClarity <= 0)
            Die();

        pc.UpdateClarity(currentClarity, maxCurrentClarity);

        UpdateColorWithHealth();
    }

    /// <summary>
    /// Changes player sprite color according to his health (is whiter if health is higher and darker if health is lower)
    /// </summary>
    private void UpdateColorWithHealth()
    {
        float c = (float)currentClarity / maxCurrentClarity;
        if (c < 0.2f) c = 0.2f;

        Color nc = new Color(c, c, c);
        //agentColor = nc;
        //agentRenderer.color = nc;

        //healthSliderColor = new Color(0f, nc.g, nc.b);
        //fillImage.color = healthSliderColor;
    }

    /// <summary>
    /// Heals x amount of health
    /// </summary>
    /// <param name="health"> Amount of health to be healed </param>
    public void HealHealth(int clarity)
    {
        currentClarity += clarity;
        if (currentClarity >= maxCurrentClarity)
            currentClarity = maxCurrentClarity;

        pc.UpdateClarity(currentClarity, maxCurrentClarity);

        UpdateColorWithHealth();
    }

    #region POWERUPS

    public void ModifyBulletSpeed(float addedSpeed)
    {
        bulletSpeed += addedSpeed;

        if (bulletSpeed > maxBulletSpeed)
            bulletSpeed = maxBulletSpeed;

        psui.ModifyBulletSpeedText(bulletSpeed);
    }

    public void ModifyBulletRange(float addedRange)
    {
        bulletRange += addedRange;

        if (bulletRange > maxBulletRange)
            bulletRange = maxBulletRange;

        psui.ModifyBulletRangeText(bulletRange);
    }

    public void ModifyPlayerSpeed(float addedSpeed)
    {
        moveSpeed += addedSpeed;

        if (moveSpeed > maxMoveSpeed)
            moveSpeed = maxMoveSpeed;

        psui.ModifyPlayerSpeedText(moveSpeed);
    }

    public void ModifyFireRate(float addedFireRate)
    {
        fireRate -= addedFireRate;

        if (fireRate <= minFireRate)
            fireRate = minFireRate;

        psui.ModifyFireRateText(fireRate);
    }

    public void ModifyDamage(int addedDamage)
    {
        bulletDamage += addedDamage;

        // I don't know if there should be a limit to the power of the player
        // if (bulletDamage > maxBulletDamage)
        //     bulletDamage = maxBulletDamage;

        psui.ModifyDamageText(bulletDamage);
    }

    public void ModifyClarity(int addedClarity)
    {
        maxCurrentClarity += addedClarity;

        if (maxCurrentClarity > lastMaxClarity)
            maxCurrentClarity = lastMaxClarity;

        psui.ModifyHealthText(currentClarity, maxCurrentClarity);
        pc.UpdateClarity(currentClarity, maxCurrentClarity);
    }

    #endregion
}
