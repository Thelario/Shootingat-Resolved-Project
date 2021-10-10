using System;
using TMPro;
using UnityEngine;

namespace PabloLario.Characters.Core.Stats
{

    // Forcing generic type to be number is not available until C# 10
    // Remove FlaotStat and intStat then
    //https://stackoverflow.com/a/68736791/14559140
    [System.Serializable]
    public abstract class Stat<T>
    {


        public abstract T Value { get; set; }

        [SerializeField]
        protected T limitValue;

        public T LimitValue { get => limitValue; }

        [SerializeField]
        protected bool improvingIncreasesValue = true;

        [SerializeField]
        private TMP_Text textUi;

        [SerializeField]
        private string textFixture;

        public delegate void OnUpdate(Stat<T> previous, Stat<T> after);
        public OnUpdate onUpdateValue;


        protected void UpdateText()
        {
            textUi.text = GetMessageToDisplay();
        }

        protected virtual String GetMessageToDisplay()
        {
            return textFixture + Value;
        }

        public abstract void RefreshValue();

        public abstract void UpgradeValue(T amount);

        public abstract void DowngradeValue(T amount);

        protected abstract Stat<T> Clone();


    }

}