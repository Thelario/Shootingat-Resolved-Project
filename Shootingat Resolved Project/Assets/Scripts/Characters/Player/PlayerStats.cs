using System.Collections;
using PabloLario.Animations;
using PabloLario.Characters.Core.Stats;
using UnityEngine;
using PabloLario.Managers;
using PabloLario.UI;

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
        [SerializeField] private GameObject walkParticlesG;
        [SerializeField] private ParticleSystem walkParticlesPS;

        [Header("Default Stats Values")]
        [SerializeField] private int defaultClarity;
        [SerializeField] private int defaultAbility;
        [SerializeField] private int defaultDamage;
        [SerializeField] private float defaultRange;
        [SerializeField] private float defaultBulletSpeed;
        [SerializeField] private float defaultFireRate;
        [SerializeField] private float defaultMoveSpeed;
        
        private void Start()
        {
            UpdateUI();

            clarity.onUpdateValue += OnClarityUpdate;
            abilityPoints.onUpdateValue += OnAbilityUpdate;
            GameManager.OnEnemyDead += UpdateAbilityAfterEnemyDie;
            GameManager.OnWinGame += Win;
            GameManager.OnDungeonGenerated += SetStatsToDefaultValues;
        }

        private void Update()
        {
            if (_invencibilityTimeWhenHitCounter > 0f)
            {
                _invencibilityTimeWhenHitCounter -= Time.deltaTime;
                if (_invencibilityTimeWhenHitCounter <= 0f)
                {
                    walkParticlesG.SetActive(true);
                    walkParticlesPS.GetComponent<ParticleSystem>().Play();
                }
            }
        }

        private void SetStatsToDefaultValues()
        {
            fireRate.Value = defaultFireRate;
            bulletStats.damageUpgradable.Value = defaultDamage;
            bulletStats.rangeUpgradable.Value = defaultRange;
            bulletStats.speedUpgradable.Value = defaultBulletSpeed;
            moveSpeed.Value = defaultMoveSpeed;
            clarity.Value = defaultClarity;
            abilityPoints.Value = defaultAbility;
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
                yield return null;
            
            _dying = true;
            SoundManager.Instance.PlaySound(SoundType.PlayerDeath);
            Time.timeScale = 0.25f;
            yield return new WaitForSecondsRealtime(2f);
            GameManager.InvokeDelegateOnLostGame();
            CanvasManager.Instance.SwitchCanvas(CanvasType.DeadGameMenu, false);
            _dying = false;
        }

        private void Win()
        {
            StartCoroutine(nameof(WinGame));
        }
        
        private IEnumerator WinGame()
        {
            SetInvencibility();
            // TODO: Play win sound
            Time.timeScale = 0.25f;
            yield return new WaitForSecondsRealtime(2f);
            CanvasManager.Instance.SwitchCanvas(CanvasType.WinGameMenu, false);
        }

        private void SetInvencibility()
        {
            _invencibilityTimeWhenHitCounter = invencibilityTimeWhenHit;
            walkParticlesG.SetActive(false);
            walkParticlesPS.GetComponent<ParticleSystem>().Stop();
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
            if (!_dying)
                Time.timeScale = 1f;
        }
    }
}