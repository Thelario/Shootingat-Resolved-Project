using UnityEngine;

public class HealthPowerup : Powerup
{
    [SerializeField] private int addedHealth;

    public override void ApplyPowerup()
    {
        if (goodPowerup)
            PlayerStats.Instance.ModifyClarity(addedHealth);
        else
            PlayerStats.Instance.ModifyClarity(-addedHealth);

        ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
        SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
        Destroy(gameObject);
    }
}
