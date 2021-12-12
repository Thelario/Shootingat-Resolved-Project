using UnityEngine;
using TMPro;

namespace PabloLario.Managers
{
    public class CountdownManager : Singleton<CountdownManager>
    {
        [SerializeField] private TMP_Text _countdownText;
        private float _timeValue;
    }
}
