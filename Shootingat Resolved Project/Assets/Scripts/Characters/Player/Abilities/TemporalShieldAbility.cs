using UnityEngine;
using PabloLario.Managers;
using PabloLario.Characters.Core.Shooting;

namespace PabloLario.Characters.Player.Abilities
{
    public class TemporalShieldAbility : Ability
    {
        protected override void Use(PlayerStats ps, PlayerController pc)
        {
            ps.abilityPoints.DowngradeValue(useCost);
            
            _abilityObject = Instantiate(GetTemporalShield(), pc.transform.position, Quaternion.identity, pc.GetWeaponTransform());

            TemporalShield ts = _abilityObject.GetComponent<TemporalShield>();
            ts.SetColorAndDestroyTime(ps.hitAnimation.agentColor, destroyAbilityTime, destroyAbility);
        }

        private GameObject GetTemporalShield()
        {
            if (Assets.Instance.abilitiesDictionary.TryGetValue(AbilityType.TemporalShield, out GameObject a))
                return a;
            else
                Debug.LogError("Temporal Shield NOT FOUND!");
            return null;
        }
    }
}