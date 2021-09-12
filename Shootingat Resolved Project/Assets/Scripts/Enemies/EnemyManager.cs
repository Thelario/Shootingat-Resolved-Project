using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private List<GameObject> enemies;

    /// <summary>
    /// Gets a random enemy from the enemies arrray
    /// </summary>
    /// <returns> Random enemy prefab to be instantiated </returns>
    public GameObject GetRandomEnemy()
    {
        int r = Random.Range(0, enemies.Count);
        return enemies[r];
    }
}
