using PabloLario.Characters.Core.Stats;
using PabloLario.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PabloLario.Characters.Core.Shooting
{
    public class BrimstoneLaser : MonoBehaviour
    {
        [Header("Fields")]
        [SerializeField] private float damageTickTime = 0.1f;

        private float _damageTickTimeCounter;
        private int _damagePerTick;

        private void Start()
        {
            _damageTickTimeCounter = damageTickTime;
        }

        private void Update()
        {
            _damageTickTimeCounter -= Time.deltaTime;
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

        public void SetDamage(int damage)
        {
            _damagePerTick = damage;
        }
    }
}