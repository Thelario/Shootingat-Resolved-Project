using PabloLario.Managers;
using PabloLario.Characters.Player;
using UnityEngine;

namespace PabloLario.Powerups
{
    public class FireRatePowerup : Powerup
    {
        [SerializeField] private float addedFireRate;

        public override void ApplyPowerup(PlayerStats ps)
        {
            if (goodPowerup)
                ps.fireRate.UpgradeValue(addedFireRate);
            else
                ps.fireRate.DowngradeValue(addedFireRate);

            ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
            SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
            Destroy(gameObject);
        }
    }
}