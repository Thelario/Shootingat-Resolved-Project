using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Managers
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        [SerializeField] private List<GameObject> enemies;

        public GameObject GetRandomEnemy()
        {
            int r = Random.Range(0, enemies.Count);
            return enemies[r];
        }
    }
}