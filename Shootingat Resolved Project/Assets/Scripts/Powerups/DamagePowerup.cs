using UnityEngine;

public class DamagePowerup : Powerup
{
    [SerializeField] private int addedDamage;

    public override void ApplyPowerup()
    {
        if (goodPowerup)
            PlayerStats.Instance.ModifyDamage(addedDamage);
        else
            PlayerStats.Instance.ModifyDamage(-addedDamage);

        ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
        SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
        Destroy(gameObject);
    }
}
