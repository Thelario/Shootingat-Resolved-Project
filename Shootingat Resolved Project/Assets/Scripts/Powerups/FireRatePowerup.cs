using UnityEngine;

public class FireRatePowerup : Powerup
{
    [SerializeField] private float addedFireRate;

    public override void ApplyPowerup()
    {
        if (goodPowerup)
            PlayerStats.Instance.ModifyFireRate(addedFireRate);
        else
            PlayerStats.Instance.ModifyFireRate(-addedFireRate);

        ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
        SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
        Destroy(gameObject);
    }
}
