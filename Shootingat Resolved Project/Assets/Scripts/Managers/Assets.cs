using UnityEngine;

// Enumeration used for saving every sound in the game.
public enum SoundType
{
    PlayerShoot,
    EnemyHit,
    PickPowerup,
    PlayerWalk,
    PlayerDash
}

// Enumeration used for saving every particleType in the game.
public enum ParticleType
{
    PlayerShoot,
    PlayerHit,
    EnemyDead,
    EnemySpawn,
    PickPowerup
}

// Enumeration used for saving all bullets according to their types.
public enum BulletType
{
    playerBullet,
    enemyBullet
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

public class Assets : Singleton<Assets>
{
    // I am thinking on changing the arrays with dictionaries, but I assume they are 
    // not going to be Serializable, which is going to be a problem for assigning the
    // resources in the inspector.

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
}
