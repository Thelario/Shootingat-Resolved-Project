using PabloLario.Managers;
using PabloLario.Characters.Player;
using UnityEngine;

namespace PabloLario.Powerups
{
    public class BulletMoveSpeedPowerup : Powerup
    {
        [SerializeField] private float bulletMoveSpeedIncrease;

        public override void ApplyPowerup(PlayerStats ps)
        {
            if (goodPowerup)
                ps.bulletSpeed.UpgradeValue(bulletMoveSpeedIncrease);
            else
                ps.bulletSpeed.DowngradeValue(bulletMoveSpeedIncrease);

            ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
            SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
            Destroy(gameObject);
        }
    }
}
