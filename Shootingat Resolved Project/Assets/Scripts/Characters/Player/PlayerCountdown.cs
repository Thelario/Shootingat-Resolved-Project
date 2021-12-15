using UnityEngine;
using TMPro;
using PabloLario.Managers;

namespace PabloLario.Characters.Player
{
    public class PlayerCountdown : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countdownText;
        [SerializeField] private TMP_Text _countdownTextWinPanel;
        [SerializeField] private TMP_Text _countdownTextDeadPanel;

        private float _time;

        private void Start()
        {
            GameManager.OnDungeonGenerated += ResetCountdown;
        }

        private void OnDestroy()
        {
            GameManager.OnDungeonGenerated -= ResetCountdown;
        }

        private void Update()
        {
            UpdateCuntdown();
        }

        private void UpdateCuntdown()
        {
            _time += Time.deltaTime;
            UpdateText();
        }

        private void ResetCountdown()
        {
            _time = 0.0f;
            UpdateText();
        }

        private void UpdateText()
        {
            _countdownText.text = "Time: " + Mathf.FloorToInt(_time);
            _countdownTextWinPanel.text = "Time: " + Mathf.FloorToInt(_time);
            _countdownTextDeadPanel.text = "Time: " + Mathf.FloorToInt(_time);
        }
    }
}
