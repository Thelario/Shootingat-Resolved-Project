using System.Collections;
using PabloLario.Animations;
using PabloLario.Characters.Core.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PabloLario.Characters.Player
{
    public class PlayerStats : MonoBehaviour, IDamageable
    {
        public UpgradableIntStatOverridableLimits clarity;

        public PlayerBulletStats bulletStats;

        public UpgradableFloatStat fireRate;

        public UpgradableFloatStat moveSpeed;


        [SerializeField] private PlayerClarity pc;

        [SerializeField] private HitColorChangeAnimation hitAnimation;

        private void Start()
        {
            UpdateUI();

            clarity.onUpdateValue += OnClarityUpdate;
        }

        private void UpdateUI()
        {
            bulletStats.RefreshValues();
            clarity.RefreshValue();
            fireRate.RefreshValue();
            moveSpeed.RefreshValue();
            pc.UpdateClarity(clarity.Value, clarity.LimitValue);
        }

        private void OnClarityUpdate(UpgradableStat<int> previousClarity, UpgradableStat<int> nextClarity)
        {
            pc.UpdateClarity(nextClarity.Value, nextClarity.LimitValue);

            if (nextClarity.Value <= 0)
                Die();

            if (nextClarity.Value < previousClarity.Value)
            {
                StartCoroutine(hitAnimation.Co_HitColorChange());
            }

        }

        private void Die()
        {
            SceneManager.LoadScene(0);
        }

        public void TakeDamage(int damage)
        {
            clarity.DowngradeValue(damage);
        }


    }
}