using UnityEngine;

public class BulletMoveSpeedPowerup : Powerup
{
    [SerializeField] private float bulletMoveSpeedIncrease;

    public override void ApplyPowerup()
    {
        if (goodPowerup)
            PlayerStats.Instance.ModifyBulletSpeed(bulletMoveSpeedIncrease);
        else
            PlayerStats.Instance.ModifyBulletSpeed(-bulletMoveSpeedIncrease);

        ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
        SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
        Destroy(gameObject);
    }
}