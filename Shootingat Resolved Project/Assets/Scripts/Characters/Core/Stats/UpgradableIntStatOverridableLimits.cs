using System.Linq;
using UnityEngine;

namespace PabloLario.Characters.Core.Stats
{
    [System.Serializable]
    public class UpgradableIntStatOverridableLimits : UpgradableIntStat
    {
        [SerializeField]
        private int maxLimit;

        public void UpgradeMaxLimit(int value)
        {
            var pre = this.Clone();
            limitValue += Mathf.Min(value, maxLimit);
            onUpdateValue?.Invoke(pre, this);
            if (Value > limitValue)
                Value = limitValue;
        }

        public void DowngradeMaxLimit(int value)
        {
            UpgradeMaxLimit(-value);
        }

        protected override object[] GetArgumentsToDisplay()
        {
            return base.GetArgumentsToDisplay().Concat(new object[] { limitValue}).ToArray();
        }
    }
}