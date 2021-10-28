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
            
            GameObject a = Instantiate(GetTemporalShield(), pc.transform.position, Quaternion.identity, pc.GetWeaponTransform());

            TemporalShield ts = a.GetComponent<TemporalShield>();
            ts.SetColorAndDestroyTime(ps.hitAnimation.agentColor, destroyAbilityTime, destroyAbility);
        }

        private GameObject GetTemporalShield()
        {
            if (Assets.Instance.abilitiesDictionary.TryGetValue(AbilityType.temporalShield, out GameObject a))
                return a;
            else
                Debug.LogError("Temporal Shield NOT FOUND!");
            return null;
        }
    }
}