using UnityEngine;
using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;

namespace PabloLario.Characters.Player.Abilities
{
    public class BrimstoneAbillity : Ability
    {
        protected override void Use(PlayerStats ps, PlayerController pc)
        {
            ps.abilityPoints.DowngradeValue(useCost);
            
            GameObject b = Instantiate(GetBrimstomeLaser(), pc.GetWeaponTransform());
            b.transform.SetPositionAndRotation(pc.GetWeaponTransform().position, pc.GetWeaponTransform().rotation);
            
            BrimstoneLaser bl = b.GetComponent<BrimstoneLaser>();
            bl.SetDamageRangeColorAndDestroyTime(ps.bulletStats.Damage, ps.bulletStats.Range, ps.hitAnimation.agentColor, destroyAbilityTime, destroyAbility);
        }

        private GameObject GetBrimstomeLaser()
        {
            if (Assets.Instance.abilitiesDictionary.TryGetValue(AbilityType.brimstoneLaser, out GameObject brimstoneLaser))
                return brimstoneLaser;
            else
                Debug.LogError("Brimstone Laser NOT FOUND!");
                return null;
        }
    }
}
