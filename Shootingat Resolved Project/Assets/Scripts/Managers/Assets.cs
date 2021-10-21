using UnityEngine;

namespace PabloLario.Managers
{
    public enum SoundType
    {
        PlayerShoot,
        EnemyHit,
        PickPowerup,
        PlayerWalk,
        PlayerDash,
        EnemyDead
    }

    public enum ParticleType
    {
        PlayerShoot,
        PlayerHit,
        EnemyDead,
        EnemySpawn,
        PickPowerup
    }

    public enum BulletType
    {
        playerBullet,
        enemyBullet,
        giantBullet,
        brimstoneLaser
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundType sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class Particle
    {
        public ParticleType type;
        public GameObject particlePrefab;
    }

    [System.Serializable]
    public class Bullets
    {
        public BulletType type;
        public GameObject bulletPrefab;
    }

    [System.Serializable]
    public class Items
    {
        public string itemName;
        public GameObject itemPrefab;
    }

    public class Assets : Singleton<Assets>
    {
        // I am thinking on changing the arrays with dictionaries, but I assume they are 
        // not going to be Serializable, which is going to be a problem for assigning the
        // resources in the inspector.

        // I might be able to have dictionaries if I use arrays or list for serialization and
        // load all the objects into a dictionary when the game starts.

        [Header("SFX")]
        public SoundAudioClip[] soundAudioClipArray;

        [Header("Particles")]
        public Particle[] particlesArray;

        [Header("Bullets")]
        public Bullets[] bulletsArray;

        [Header("Player Reference")]
        public Transform playerTransform;

        [Header("Splash Images")]
        public GameObject bulletSplash_1;
        public GameObject bloodSplash_1;

        [Header("Items")]
        public Items[] itemsArray;

        [Header("Score Floating Text")]
        public GameObject damageFloatingText;
    }
}
