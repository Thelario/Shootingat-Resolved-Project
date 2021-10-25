using UnityEngine;
using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;

namespace PabloLario.Characters.Player.Abilities
{
    public class BrimstoneAbillity : Ability
    {
        public override void UseAbility(PlayerStats ps, PlayerController pc)
        {
            if (ps.abilityPoints.Value >= useCost)
            {
                ps.abilityPoints.DowngradeValue(useCost);
                GameObject b = Instantiate(GetBrimstomeLaser(), pc.GetWeaponTransform());
                b.transform.SetPositionAndRotation(pc.GetWeaponTransform().position, pc.GetWeaponTransform().rotation);
                b.GetComponent<BrimstoneLaser>().SetDamage(ps.bulletStats.Damage);
                SpriteRenderer sr = b.GetComponent<SpriteRenderer>();
                sr.size = new Vector2(sr.size.x, ps.bulletStats.Range);
                sr.color = ps.hitAnimation.agentColor;
                Destroy(b, 5f);
            }
        }

        private GameObject GetBrimstomeLaser()
        {
            if (Assets.Instance.bulletsDictionary.TryGetValue(BulletType.brimstoneLaser, out GameObject brimstoneLaser))
                return brimstoneLaser;
            else
                Debug.LogError("Brimstone Laser NOT FOUND!");
                return null;
        }
    }
}
