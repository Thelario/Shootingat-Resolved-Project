using UnityEngine;

namespace PabloLario.Characters.Core.Stats
{
    [System.Serializable]
    public class FloatStat : Stat<float>
    {
        [SerializeField]
        private float _value;

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
                onUpdateValue?.Invoke(previous, this);

                UpdateText();
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
                Value = Value + amount;
            }
            else
            {
                Value = Value - amount;
            }
        }

        public override void DowngradeValue(float amount)
        {
            UpgradeValue(-amount);
        }

        protected override Stat<float> Clone()
        {
            FloatStat stat = new FloatStat();
            stat._value = Value;
            stat.limitValue = limitValue;
            return stat;
        }

    }
}