using UnityEngine;

namespace PabloLario.Powerups
{
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] protected bool goodPowerup = true;

        public abstract void ApplyPowerup();
    }
}
