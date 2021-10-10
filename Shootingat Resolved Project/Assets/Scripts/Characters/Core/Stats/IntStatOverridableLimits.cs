using UnityEngine;

namespace PabloLario.Characters.Core.Stats
{
    [System.Serializable]
    public class IntStatOverridableLimits : IntStat
    {

        [SerializeField]
        private int maxLimit;


        public void UpgradeMaxLimit(int value)
        {
            var pre = this.Clone();
            limitValue += Mathf.Min(value, maxLimit);
            onUpdateValue?.Invoke(pre, this);
        }

        public void DowngradeMaxLimit(int value)
        {
            UpgradeMaxLimit(-value);
        }

        protected override string GetMessageToDisplay()
        {
            return base.GetMessageToDisplay() + "/" + limitValue;
        }


    }
}