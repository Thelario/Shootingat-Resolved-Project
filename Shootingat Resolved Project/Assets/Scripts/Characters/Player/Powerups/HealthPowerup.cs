using PabloLario.Managers;
using PabloLario.Characters.Player;
using UnityEngine;

namespace PabloLario.Characters.Player.Powerups
{
    public class HealthPowerup : Powerup
    {
        [SerializeField] private int addedHealth;

        public override void ApplyPowerup(PlayerStats ps)
        {
            if (goodPowerup)
                ps.clarity.UpgradeMaxLimit(addedHealth);
            else
                ps.clarity.DowngradeMaxLimit(addedHealth);

            ParticlesManager.Instance.CreateParticle(ParticleType.PickPowerup, transform.position, 0.5f, Quaternion.Euler(90f, 0f, 0f));
            SoundManager.Instance.PlaySound(SoundType.PickPowerup, 0.5f);
            Destroy(gameObject);
        }
    }
}