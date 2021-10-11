using PabloLario.Managers;
using PabloLario.Characters.Player;
using UnityEngine;

namespace PabloLario.Characters.Player.Powerups
{
    public class PlayerMoveSpeedPowerup : Powerup
    {
        [SerializeField] private float addedPlayerMoveSpeed;

        public override void ApplyPowerup(PlayerStats ps)
        {
            if (goodPowerup)
                ps.moveSpeed.UpgradeValue(addedPlayerMoveSpeed);
            else
                ps.moveSpeed.DowngradeValue(addedPlayerMoveSpeed);

            ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
            SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
            Destroy(gameObject);
        }
    }
}