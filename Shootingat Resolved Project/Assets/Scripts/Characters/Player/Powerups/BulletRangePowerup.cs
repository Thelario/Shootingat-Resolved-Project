using PabloLario.Managers;
using PabloLario.Characters.Player;
using UnityEngine;

namespace PabloLario.Powerups
{
    public class BulletRangePowerup : Powerup
    {
        [SerializeField] private float addedRange;

        public override void ApplyPowerup(PlayerStats ps)
        {
            if (goodPowerup)
                ps.bulletStats.rangeUpgradable.UpgradeValue(addedRange);
            else
                ps.bulletStats.rangeUpgradable.DowngradeValue(addedRange);

            ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
            SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
            Destroy(gameObject);
        }
    }
}