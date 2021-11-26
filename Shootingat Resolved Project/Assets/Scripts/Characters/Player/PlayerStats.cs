using System.Collections;
using PabloLario.Animations;
using PabloLario.Characters.Core.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;
using PabloLario.Managers;

namespace PabloLario.Characters.Player
{
    public class PlayerStats : MonoBehaviour, IDamageable
    {
        public UpgradableIntStatOverridableLimits clarity;
        public PlayerBulletStats bulletStats;
        public UpgradableFloatStat fireRate;
        public UpgradableFloatStat moveSpeed;
        public UpgradableIntStatOverridableLimits abilityPoints;

        [SerializeField] private PlayerClarity pc;
        [SerializeField] private PlayerAbilityPoints pap;

        public HitColorChangeAnimation hitAnimation;

        private bool _dying = false;

        private void Start()
        {
            UpdateUI();

            clarity.onUpdateValue += OnClarityUpdate;
            abilityPoints.onUpdateValue += OnAbilityUpdate;
            GameManager.OnEnemyDead += UpdateAbilityAfterEnemyDie;
        }

        private void UpdateUI()
        {
            bulletStats.RefreshValues();
            clarity.RefreshValue();
            fireRate.RefreshValue();
            moveSpeed.RefreshValue();
            pc.UpdateClarity(clarity.Value, clarity.LimitValue);
            pap.UpdateAbility(abilityPoints.Value, abilityPoints.LimitValue);
        }

        private void OnAbilityUpdate(UpgradableStat<int> previousAbility, UpgradableStat<int> nextAbility)
        {
            pap.UpdateAbility(nextAbility.Value, nextAbility.LimitValue);
        }

        private void UpdateAbilityAfterEnemyDie(int abilityPoints)
        {
            this.abilityPoints.UpgradeValue(abilityPoints);
        }

        private void OnClarityUpdate(UpgradableStat<int> previousClarity, UpgradableStat<int> nextClarity)
        {
            pc.UpdateClarity(nextClarity.Value, nextClarity.LimitValue);

            if (nextClarity.Value <= 0 && !_dying)
                StartCoroutine(nameof(Die));

            if (nextClarity.Value < previousClarity.Value)
            {
                StartCoroutine(hitAnimation.Co_HitColorChange());
            }
        }

        private IEnumerator Die()
        {
            _dying = true;
            SoundManager.Instance.PlaySound(SoundType.PlayerDeath);
            Time.timeScale = 0.25f;
            yield return new WaitForSeconds(2f);
            // TODO: After that time, load a screen with a death message
            // TODO: Give some options to player (go back to menu, restart game)
            _dying = false;
        }

        public void TakeDamage(int damage)
        {
            clarity.DowngradeValue(damage);

            StartCoroutine(nameof(FreezeTime));
        }

        private IEnumerator FreezeTime()
        {
            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(0.1f);

            Time.timeScale = 1f;
        }
    }
}