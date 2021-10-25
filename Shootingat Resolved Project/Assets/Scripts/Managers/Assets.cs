using UnityEngine;
using System.Collections.Generic;

namespace PabloLario.Managers
{
    public enum SoundType
    {
        PlayerShoot,
        EnemyHit,
        PickPowerup,
        PlayerWalk,
        PlayerDash,
        EnemyDead,
        Blop
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
        [SerializeField] private SoundAudioClip[] soundAudioClipArray;
        public Dictionary<SoundType, AudioClip> soundAudioClipDictionary;

        [Header("Particles")]
        [SerializeField] private Particle[] particlesArray;
        public Dictionary<ParticleType, GameObject> particlesDictionary;

        [Header("Bullets")]
        [SerializeField] private Bullets[] bulletsArray;
        public Dictionary<BulletType, GameObject> bulletsDictionary;

        [Header("Player Reference")]
        public Transform playerTransform;

        [Header("Splash Images")]
        public GameObject bulletSplash_1;
        public GameObject bloodSplash_1;

        [Header("Items")]
        public Items[] itemsArray;
        public Dictionary<string, GameObject> itemsDictionary;

        [Header("Score Floating Text")]
        public GameObject damageFloatingText;

        protected override void Awake()
        {
            base.Awake();

            PopulateSoundAudioClipDictionary();
            PopulateParticlesDictionary();
            PopulateBulletsDictionary();
            PopulateItemsDictionary();
        }

        private void PopulateSoundAudioClipDictionary()
        {
            soundAudioClipDictionary = new Dictionary<SoundType, AudioClip>();

            foreach (SoundAudioClip s in soundAudioClipArray)
            {
                soundAudioClipDictionary.Add(s.sound, s.audioClip);
            }
        }

        private void PopulateParticlesDictionary()
        {
            particlesDictionary = new Dictionary<ParticleType, GameObject>();

            foreach (Particle p in particlesArray)
            {
                particlesDictionary.Add(p.type, p.particlePrefab);
            }
        }

        private void PopulateBulletsDictionary()
        {
            bulletsDictionary = new Dictionary<BulletType, GameObject>();

            foreach (Bullets b in bulletsArray)
            {
                bulletsDictionary.Add(b.type, b.bulletPrefab);
            }
        }

        private void PopulateItemsDictionary()
        {
            itemsDictionary = new Dictionary<string, GameObject>();

            foreach (Items i in itemsArray)
            {
                itemsDictionary.Add(i.itemName, i.itemPrefab);
            }
        }
    }
}
