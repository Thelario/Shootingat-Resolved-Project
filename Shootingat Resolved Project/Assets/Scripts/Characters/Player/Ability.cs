using UnityEngine;

namespace PabloLario.Characters.Player
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] protected int useCost;
        [SerializeField] protected bool destroyAbility = true;
        [SerializeField] protected float destroyAbilityTime = 5f;

        public Sprite abilitySprite;

        protected GameObject _abilityObject;

        public void UseAbility(PlayerStats ps, PlayerController pc)
        {
            if (ps.abilityPoints.Value >= useCost)
            {
                Use(ps, pc);
            }
        }

        public void DestroyAbilityObjectInstantly()
        {
            if (_abilityObject != null)
                Destroy(_abilityObject);
        }

        protected abstract void Use(PlayerStats ps, PlayerController pc);
    }
}
