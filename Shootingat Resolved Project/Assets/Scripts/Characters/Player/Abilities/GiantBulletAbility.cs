using UnityEngine;
using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;

namespace PabloLario.Characters.Player.Abilities
{
    public class GiantBulletAbility : Ability
    {
        public override void UseAbility(PlayerStats ps, PlayerController pc)
        {
            if (ps.abilityPoints.Value >= useCost)
            {
                ps.abilityPoints.DowngradeValue(useCost);
                GameObject b = Instantiate(GetGiantBullet());
                b.transform.SetPositionAndRotation(pc.GetShootPointTransform().position, pc.GetShootPointTransform().rotation);
                b.GetComponent<Bullet>().SetDirStatsColor(pc.dir, ps.bulletStats, ps.hitAnimation.agentColor);
                SoundManager.Instance.PlaySound(SoundType.PlayerShoot, 2f);
                Destroy(b, 15f);
            }
        }

        private GameObject GetGiantBullet()
        {
            if (Assets.Instance.bulletsDictionary.TryGetValue(BulletType.giantBullet, out GameObject bullet))
                return bullet;
            else
                Debug.LogError("Giant Bullet NOT FOUND!");
                return null;
        }
    }
}
