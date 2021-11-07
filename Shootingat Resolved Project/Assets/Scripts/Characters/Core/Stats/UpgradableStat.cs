using PabloLario.UI;
using UnityEngine;
using System;

namespace PabloLario.Characters.Core.Stats
{
    // Forcing generic type to be number is not available until C# 10
    // Remove FlaotStat and intStat then
    // https://stackoverflow.com/a/68736791/14559140
    [System.Serializable]
    public abstract class UpgradableStat<T>
    {
        public abstract T Value { get; set; }

        [SerializeField] protected T limitValue;

        public T LimitValue { get => limitValue; }

        [SerializeField] protected bool improvingIncreasesValue = true;

        [SerializeField] private UIFormatter textUiFormatter;

        public delegate void OnUpdate(UpgradableStat<T> previous, UpgradableStat<T> after);
        public OnUpdate onUpdateValue;

        protected void UpdateText()
        {
            if (textUiFormatter != null)
                textUiFormatter.UpdateText(GetArgumentsToDisplay());
        }

        protected virtual object[] GetArgumentsToDisplay()
        {
            return new object[] {Value};
        }

        public abstract void RefreshValue();

        public abstract void UpgradeValue(T amount);

        public abstract void DowngradeValue(T amount);

        protected abstract UpgradableStat<T> Clone();
    }
}