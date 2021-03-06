using UnityEngine;

namespace PabloLario.Characters.Core.Stats
{
    [System.Serializable]
    public class UpgradableIntStat : UpgradableStat<int>
    {
        [SerializeField] private int _value;

        private bool _good;

        public override int Value
        {
            get { return _value; }
            set
            {
                var previous = this.Clone();

                _value = value;

                if (IsBeyondLimit())
                {
                    _value = limitValue;
                }

                UpdateText(_good);
                
                onUpdateValue?.Invoke(previous, this);
            }
        }

        private bool IsBeyondLimit()
        {
            return improvingIncreasesValue && _value > limitValue || !improvingIncreasesValue && _value < limitValue;
        }

        public override void RefreshValue()
        {
            Value = _value;
        }

        public override void UpgradeValue(int amount)
        {
            if (improvingIncreasesValue)
            {
                if (amount < 0f)
                    _good = false;
                else
                    _good = true;

                Value = Value + amount;
            }
            else
            {
                if (amount > 0f)
                    _good = false;
                else
                    _good = true;

                Value = Value - amount;
            }
        }

        public override void DowngradeValue(int amount)
        {
            UpgradeValue(-amount);
        }

        protected override UpgradableStat<int> Clone()
        {
            UpgradableIntStat stat = new UpgradableIntStat();
            stat._value = Value;
            stat.limitValue = limitValue;
            return stat;
        }
    }
}