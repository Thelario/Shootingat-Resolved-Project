using PabloLario.Managers;
using PabloLario.Characters.Player;
using UnityEngine;

namespace PabloLario.Powerups
{
    public class DamagePowerup : Powerup
    {
        [SerializeField] private int addedDamage;

        public override void ApplyPowerup(PlayerStats ps)
        {
            if (goodPowerup)
                ps.bulletStats.damageUpgradable.UpgradeValue(addedDamage);
            else
                ps.bulletStats.damageUpgradable.DowngradeValue(addedDamage);

            ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
            SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
            Destroy(gameObject);
        }
    }
}