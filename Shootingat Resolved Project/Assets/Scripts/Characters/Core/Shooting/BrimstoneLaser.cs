using PabloLario.Characters.Core.Stats;
using PabloLario.Managers;
using TMPro;
using UnityEngine;

namespace PabloLario.Characters.Core.Shooting
{
    public class BrimstoneLaser : MonoBehaviour
    {
        [Header("Fields")]
        [SerializeField] private float damageTickTime = 0.1f;
        
        private SpriteRenderer _sr;

        private float _damageTickTimeCounter;
        private int _damagePerTick;
        private float _destroyTime;
        private float _destroyTimeCounter = -1f;
        private bool _destroyAbility;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _damageTickTimeCounter = damageTickTime;
        }

        private void Update()
        {
            if (_destroyTimeCounter < 0f)
                return;

            _damageTickTimeCounter -= Time.deltaTime;

            if (!_destroyAbility)
                return;

            _destroyTimeCounter += Time.deltaTime;
            if (_destroyTimeCounter >= _destroyTime)
                Destroy(gameObject);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet") || collision.CompareTag("EnemyTrigger") || collision.CompareTag("Door") || collision.CompareTag("Player"))
                return;

            if (_damageTickTimeCounter > 0f)
                return;

            if (!collision.TryGetComponent(out IDamageable id))
                return;

            _damageTickTimeCounter = damageTickTime;
            id.TakeDamage(_damagePerTick);
            SoundManager.Instance.PlaySound(SoundType.EnemyHit, 0.5f);
            GameObject damageFloatingText = Instantiate(Assets.Instance.damageFloatingText, collision.transform.position + Vector3.up, Quaternion.identity);
            damageFloatingText.GetComponent<TMP_Text>().text = _damagePerTick.ToString();
        }

        public void SetDamageRangeColorAndDestroyTime(int d, float r, Color c, float t, bool dt)
        {
            _damagePerTick = d;
            _sr.size = new Vector2(_sr.size.x, r);
            _sr.color = c;
            _destroyTime = t;
            _destroyAbility = dt;
            _destroyTimeCounter = 0f;
        }
    }
}