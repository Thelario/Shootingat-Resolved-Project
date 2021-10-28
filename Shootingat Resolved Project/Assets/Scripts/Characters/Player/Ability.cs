using UnityEngine;

namespace PabloLario.Characters.Player
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] protected int useCost;
        [SerializeField] protected bool destroyAbility = true;
        [SerializeField] protected float destroyAbilityTime = 5f;

        public Sprite abilitySprite;

        public void UseAbility(PlayerStats ps, PlayerController pc)
        {
            if (ps.abilityPoints.Value >= useCost)
            {
                Use(ps, pc);
            }
        }

        protected abstract void Use(PlayerStats ps, PlayerController pc);
    }
}
