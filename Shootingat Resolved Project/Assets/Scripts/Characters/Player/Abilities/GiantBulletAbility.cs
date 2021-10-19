using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PabloLario.Managers;

namespace PabloLario.Characters.Player.Abilities
{
    public class GiantBulletAbility : Ability
    {
        public override void UseAbility(PlayerStats ps, PlayerController pc)
        {
            if (ps.abilityPoints.Value >= useCost)
            {
                ps.abilityPoints.DowngradeValue(useCost);
                // TODO: Do the ability functionality
                print("Using Giant Bullet Ability");
            }
            else print("Not enough abilityPoints to use");
        }
    }
}
