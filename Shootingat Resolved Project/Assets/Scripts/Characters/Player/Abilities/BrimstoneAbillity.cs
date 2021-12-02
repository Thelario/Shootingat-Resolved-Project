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

            _abilityObject = Instantiate(GetBrimstomeLaser(), pc.GetWeaponTransform());
            _abilityObject.transform.SetPositionAndRotation(pc.GetWeaponTransform().position, pc.GetWeaponTransform().rotation);
            
            BrimstoneLaser bl = _abilityObject.GetComponent<BrimstoneLaser>();
            bl.SetDamageRangeColorAndDestroyTime(ps.bulletStats.Damage, ps.bulletStats.Range, ps.hitAnimation.agentColor, destroyAbilityTime, destroyAbility);
            
            SoundManager.Instance.PlaySound(SoundType.Laser);
        }

        private GameObject GetBrimstomeLaser()
        {
            if (Assets.Instance.abilitiesDictionary.TryGetValue(AbilityType.BrimstoneLaser, out GameObject brimstoneLaser))
                return brimstoneLaser;
            else
                Debug.LogError("Brimstone Laser NOT FOUND!");
                return null;
        }
    }
}
