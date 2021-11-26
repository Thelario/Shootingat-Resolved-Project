using UnityEngine;
using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;

namespace PabloLario.Characters.Player.Abilities
{
    public class GiantBulletAbility : Ability
    {
        protected override void Use(PlayerStats ps, PlayerController pc)
        {
            ps.abilityPoints.DowngradeValue(useCost);
            
            GameObject b = Instantiate(GetGiantBullet());
            b.transform.SetPositionAndRotation(pc.GetShootPointTransform().position, pc.GetShootPointTransform().rotation);

            GiantBullet gb = b.GetComponent<GiantBullet>();
            gb.SetDirStatsColor(pc.dir, ps.bulletStats, ps.hitAnimation.agentColor);
            gb.SetDestroyTime(destroyAbilityTime, destroyAbility);
            
            SoundManager.Instance.PlaySound(SoundType.PlayerShoot, 3f);
        }

        private GameObject GetGiantBullet()
        {
            if (Assets.Instance.bulletsDictionary.TryGetValue(BulletType.GigantBullet, out GameObject bullet))
                return bullet;
            else
                Debug.LogError("Giant Bullet NOT FOUND!");
                return null;
        }
    }
}
