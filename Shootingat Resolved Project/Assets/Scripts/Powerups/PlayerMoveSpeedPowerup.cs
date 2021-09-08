using UnityEngine;

public class PlayerMoveSpeedPowerup : Powerup
{
    [SerializeField] private float addedPlayerMoveSpeed;

    public override void ApplyPowerup()
    {
        if (goodPowerup)
            PlayerStats.Instance.ModifyPlayerSpeed(addedPlayerMoveSpeed);
        else
            PlayerStats.Instance.ModifyPlayerSpeed(-addedPlayerMoveSpeed);

        ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
        SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
        Destroy(gameObject);
    }
}
