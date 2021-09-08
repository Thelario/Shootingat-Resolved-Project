using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : Singleton<BulletPoolManager>
{
    [SerializeField] private int initialPlayerBullets;
    [SerializeField] private int initialEnemyBullets;
    [SerializeField] private Transform bulletsParent;

    private List<GameObject> playerBulletsPool = new List<GameObject>();
    private List<GameObject> enemyBulletsPool = new List<GameObject>();

    private void Start()
    {
        playerBulletsPool = GeneratePlayerBullets(initialPlayerBullets);
        enemyBulletsPool = GenerateEnemyBullets(initialEnemyBullets);
    }

    /// <summary>
    /// Method for generating some bullets and add them into the bulletPool for later use
    /// </summary>
    /// <param name="amountOfBullets"> Amount of bullets to generate </param>
    /// <returns> The list of all the bullets created </returns>
    private List<GameObject> GeneratePlayerBullets(int amountOfBullets)
    {
        for (int i = 0; i < amountOfBullets; i++)
        {
            GameObject bullet = Instantiate(BulletsManager.Instance.GetBullets(BulletType.playerBullet), bulletsParent); // We create the bullet and assign the parent (clean inspector)
            playerBulletsPool.Add(bullet); // We add the bullet into the bullet pool
            bullet.SetActive(false); // Disable the bullet because we only need to activate a bullet when we shoot
        }

        return playerBulletsPool;
    }

    /// <summary>
    /// Method for searching a bullet to be shot
    /// </summary>
    /// <returns> The bullet to be shot </returns>
    public GameObject RequestPlayerBullet()
    {
        // Search for an active bullet (one that is not being used)
        foreach(GameObject b in playerBulletsPool)
        {
            if (!b.activeInHierarchy)
            {
                b.SetActive(true);
                return b;
            }
        }

        // In case more bullets are needed, we can generate them
        playerBulletsPool = GenerateEnemyBullets(1);
        return RequestPlayerBullet();
    }

    /// <summary>
    /// Method for generating some bullets and add them into the bulletPool for later use
    /// </summary>
    /// <param name="amountOfBullets"> Amount of bullets to generate </param>
    /// <returns> The list of all the bullets created </returns>
    private List<GameObject> GenerateEnemyBullets(int amountOfBullets)
    {
        for (int i = 0; i < amountOfBullets; i++)
        {
            GameObject bullet = Instantiate(BulletsManager.Instance.GetBullets(BulletType.enemyBullet), bulletsParent); // We create the bullet and assign the parent (clean inspector)
            enemyBulletsPool.Add(bullet); // We add the bullet into the bullet pool
            bullet.SetActive(false); // Disable the bullet because we only need to activate a bullet when we shoot
        }

        return enemyBulletsPool;
    }

    /// <summary>
    /// Method for searching a bullet to be shot
    /// </summary>
    /// <returns> The bullet to be shot </returns>
    public GameObject RequestEnemyBullet()
    {
        // Search for an active bullet (one that is not being used)
        foreach (GameObject b in enemyBulletsPool)
        {
            if (!b.activeInHierarchy)
            {
                b.SetActive(true);
                return b;
            }
        }

        // In case more bullets are needed, we can generate them
        enemyBulletsPool = GenerateEnemyBullets(1);
        return RequestEnemyBullet();
    }
}
