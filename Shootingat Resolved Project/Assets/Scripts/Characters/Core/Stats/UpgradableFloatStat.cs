using UnityEngine;

namespace PabloLario.Characters.Core.Stats
{
    [System.Serializable]
    public class UpgradableFloatStat : UpgradableStat<float>
    {
        [SerializeField] private float _value;

        private bool _good;

        public override float Value
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

        public override void UpgradeValue(float amount)
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

        public override void DowngradeValue(float amount)
        {
            UpgradeValue(-amount);
        }

        protected override UpgradableStat<float> Clone()
        {
            UpgradableFloatStat stat = new UpgradableFloatStat();
            stat._value = Value;
            stat.limitValue = limitValue;
            return stat;
        }
    }
}