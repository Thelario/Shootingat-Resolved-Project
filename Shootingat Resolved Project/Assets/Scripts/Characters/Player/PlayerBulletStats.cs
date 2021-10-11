using PabloLario.Characters.Core.Shooting;
using PabloLario.Characters.Core.Stats;
using UnityEngine.Serialization;

namespace PabloLario.Characters.Player
{
    [System.Serializable]
    public class PlayerBulletStats : BulletStats
    {

        public UpgradableIntStat damageUpgradable;
        public override int Damage { get => damageUpgradable.Value; set => damageUpgradable.Value = value; }

        public UpgradableFloatStat rangeUpgradable;
        public override float Range { get => rangeUpgradable.Value; set => rangeUpgradable.Value = value; }

        public UpgradableFloatStat speedUpgradable;

        public override float Speed { get => speedUpgradable.Value; set => speedUpgradable.Value = value; }

        public void RefreshValues()
        {
            damageUpgradable.RefreshValue();
            speedUpgradable.RefreshValue();
            damageUpgradable.RefreshValue();
        }

    }

}
