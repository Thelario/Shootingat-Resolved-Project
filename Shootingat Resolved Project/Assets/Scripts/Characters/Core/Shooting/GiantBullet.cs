﻿using PabloLario.Characters.Core.Stats;
using PabloLario.Managers;
using UnityEngine;
using TMPro;

namespace PabloLario.Characters.Core.Shooting
{
    public class GiantBullet : Bullet
    {
        [SerializeField] private float damageTickTime = 0.1f;

        private float _damageTickTimeCounter;

        private void Start()
        {
            _damageTickTimeCounter = damageTickTime;
        }

        protected override void Update()
        {
            base.Update();

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
            id.TakeDamage(_stats.Damage);
            SoundManager.Instance.PlaySound(SoundType.EnemyHit, 0.5f);
            GameObject damageFloatingText = Instantiate(Assets.Instance.damageFloatingText, collision.transform.position + Vector3.up, Quaternion.identity);
            damageFloatingText.GetComponent<TMP_Text>().text = _stats.Damage.ToString();
        }
    }
}
