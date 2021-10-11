using PabloLario.Characters.Player;
using UnityEngine;

namespace PabloLario.Characters.Player.Powerups
{
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] protected bool goodPowerup = true;

        public abstract void ApplyPowerup(PlayerStats playerStats);
    }
}
