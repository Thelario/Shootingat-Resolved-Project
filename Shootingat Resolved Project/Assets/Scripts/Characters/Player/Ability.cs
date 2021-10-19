using UnityEngine;

namespace PabloLario.Characters.Player
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] protected int useCost;

        public Sprite abilitySprite;

        public abstract void UseAbility(PlayerStats ps, PlayerController pc);
    }
}
