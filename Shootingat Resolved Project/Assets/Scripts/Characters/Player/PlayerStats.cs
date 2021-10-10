using System.Collections;
using PabloLario.Characters.Core.Stats;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PabloLario.Characters.Player
{
    public class PlayerStats : MonoBehaviour, IDamageable
    {
        public IntStatOverridableLimits clarity;

        public FloatStat bulletSpeed;

        public FloatStat fireRate;

        public IntStat bulletDamage;

        public FloatStat bulletRange;

        public FloatStat moveSpeed;


        [Header("References")]
        [SerializeField] private PlayerClarity pc;
        [SerializeField] protected SpriteRenderer agentRenderer;

        [Header("Color Change when Hit")]
        [SerializeField] protected Color agentColor;
        [SerializeField] protected Color hitColor;
        [SerializeField] protected float timeToWaitForColorChange;

        private void Start()
        {
            clarity.RefreshValue();
            bulletSpeed.RefreshValue();
            fireRate.RefreshValue();
            bulletDamage.RefreshValue();
            bulletRange.RefreshValue();
            moveSpeed.RefreshValue();
            pc.UpdateClarity(clarity.Value, clarity.LimitValue);

            clarity.onUpdateValue += OnClarityUpdate;
        }

        private void OnClarityUpdate(Stat<int> previousClarity, Stat<int> nextClarity){
            pc.UpdateClarity(nextClarity.Value, nextClarity.LimitValue);

            if (nextClarity.Value <= 0)
                Die();

            if(nextClarity.Value < previousClarity.Value){
                StartCoroutine(Co_HitColorChange());
            }

        }

        public void Die()
        {
            SceneManager.LoadScene(0);
        }

        public void TakeDamage(int damage)
        {
            clarity.DowngradeValue(damage);
        }

        private IEnumerator Co_HitColorChange()
        {
            agentRenderer.color = hitColor;

            yield return new WaitForSeconds(timeToWaitForColorChange);

            agentRenderer.color = agentColor;
        }

    }
}