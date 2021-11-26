using System.Collections.Generic;
using UnityEngine;

namespace PabloLario.Managers
{
    public class BulletPoolManager : Singleton<BulletPoolManager>
    {
        [SerializeField] private int initialPlayerBullets;
        [SerializeField] private int initialEnemyBullets;
        [SerializeField] private Transform bulletsParent;

        private List<GameObject> _playerBulletsPool = new List<GameObject>();
        private List<GameObject> _enemyBulletsPool = new List<GameObject>();

        private void Start()
        {
            _playerBulletsPool = GeneratePlayerBullets(initialPlayerBullets);
            _enemyBulletsPool = GenerateEnemyBullets(initialEnemyBullets);
        }

        private List<GameObject> GeneratePlayerBullets(int amountOfBullets)
        {
            for (int i = 0; i < amountOfBullets; i++)
            {
                GameObject bullet = Instantiate(BulletsManager.Instance.GetBullets(BulletType.PlayerBullet), bulletsParent); // We create the bullet and assign the parent (clean inspector)
                _playerBulletsPool.Add(bullet); // We add the bullet into the bullet pool
                bullet.SetActive(false); // Disable the bullet because we only need to activate a bullet when we shoot
            }

            return _playerBulletsPool;
        }

        public GameObject RequestPlayerBullet()
        {
            // Search for an active bullet (one that is not being used)
            foreach (GameObject b in _playerBulletsPool)
            {
                if (!b.activeInHierarchy)
                {
                    b.SetActive(true);
                    return b;
                }
            }

            // In case more bullets are needed, we can generate them
            _playerBulletsPool = GenerateEnemyBullets(1);
            return RequestPlayerBullet();
        }

        private List<GameObject> GenerateEnemyBullets(int amountOfBullets)
        {
            for (int i = 0; i < amountOfBullets; i++)
            {
                GameObject bullet = Instantiate(BulletsManager.Instance.GetBullets(BulletType.EnemyBullet), bulletsParent); // We create the bullet and assign the parent (clean inspector)
                _enemyBulletsPool.Add(bullet); // We add the bullet into the bullet pool
                bullet.SetActive(false); // Disable the bullet because we only need to activate a bullet when we shoot
            }

            return _enemyBulletsPool;
        }

        public GameObject RequestEnemyBullet()
        {
            // Search for an active bullet (one that is not being used)
            foreach (GameObject b in _enemyBulletsPool)
            {
                if (!b.activeInHierarchy)
                {
                    b.SetActive(true);
                    return b;
                }
            }

            // In case more bullets are needed, we can generate them
            _enemyBulletsPool = GenerateEnemyBullets(1);
            return RequestEnemyBullet();
        }
    }
}