using PabloLario.Managers;

namespace PabloLario.Characters.Player.Abilities
{
    public class HealAbility : Ability
    {
        protected override void Use(PlayerStats ps, PlayerController pc)
        {
            ps.abilityPoints.DowngradeValue(useCost);
            ps.clarity.Value += 1;
            SoundManager.Instance.PlaySound(SoundType.Heal);
        }
    }
}
