using System;
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

        private bool _dying;
        private bool _freezing;
        
        [Header("Invencibility When Hit")] 
        [SerializeField] private float invencibilityTimeWhenHit;
        private float _invencibilityTimeWhenHitCounter;

        private void Start()
        {
            UpdateUI();

            clarity.onUpdateValue += OnClarityUpdate;
            abilityPoints.onUpdateValue += OnAbilityUpdate;
            GameManager.OnEnemyDead += UpdateAbilityAfterEnemyDie;
        }

        private void Update()
        {
            if (_invencibilityTimeWhenHitCounter > 0f)
                _invencibilityTimeWhenHitCounter -= Time.deltaTime;
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

        private void UpdateAbilityAfterEnemyDie(int abiPoints)
        {
            abilityPoints.UpgradeValue(abiPoints);
        }

        private void OnClarityUpdate(UpgradableStat<int> previousClarity, UpgradableStat<int> nextClarity)
        {
            pc.UpdateClarity(nextClarity.Value, nextClarity.LimitValue);

            if (nextClarity.Value <= 0 && !_dying)
                StartCoroutine(nameof(Die));

            if (nextClarity.Value < previousClarity.Value)
            {
                StartCoroutine(hitAnimation.Co_HitColorChange(true, invencibilityTimeWhenHit));
                SetInvencibility();
            }
        }

        private IEnumerator Die()
        {
            while (!_freezing)
                yield return new WaitForSeconds(0.01f);
            
            _dying = true;
            SoundManager.Instance.PlaySound(SoundType.PlayerDeath);
            Time.timeScale = 0.25f;
            yield return new WaitForSeconds(2f);
            // TODO: After that time, load a screen with a death message
            // TODO: Give some options to player (go back to menu, restart game)
            _dying = false;
        }

        private void SetInvencibility()
        {
            _invencibilityTimeWhenHitCounter = invencibilityTimeWhenHit;
        }

        public bool TakeDamage(int damage)
        {
            if (_invencibilityTimeWhenHitCounter > 0f)
                return false;
            
            clarity.DowngradeValue(damage);

            StartCoroutine(nameof(FreezeTime));
            return true;
        }

        private IEnumerator FreezeTime()
        {
            _freezing = true;
            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(0.1f);

            _freezing = false;
            Time.timeScale = 1f;
        }
    }
}