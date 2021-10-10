using System.Collections;
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


        [Header("References")]
        [SerializeField] private PlayerClarity pc;
        [SerializeField] protected SpriteRenderer agentRenderer;

        [Header("Color Change when Hit")]
        [SerializeField] protected Color agentColor;
        [SerializeField] protected Color hitColor;
        [SerializeField] protected float timeToWaitForColorChange;

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
                StartCoroutine(Co_HitColorChange());
            }

        }

        private void Die()
        {
            SceneManager.LoadScene(0);
        }

        private IEnumerator Co_HitColorChange()
        {
            agentRenderer.color = hitColor;

            yield return new WaitForSeconds(timeToWaitForColorChange);

            agentRenderer.color = agentColor;
        }

        public void TakeDamage(int damage)
        {
            clarity.DowngradeValue(damage);
        }


    }
}