using System.Collections;
using PabloLario.Characters.Core.Stats;
using PabloLario.Managers;
using UnityEngine;

namespace PabloLario.Characters.Core.Shooting
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletType type; // Type of the bullet
        [SerializeField] private float enemyBulletMoveSpeed;
        [SerializeField] private float enemyBulletRange;
        [SerializeField] private SpriteRenderer sr;

        private Vector2 _dir = Vector2.zero; // Direction to point to
        private Vector2 _initialPos = Vector2.zero;

        private BulletStats _stats;

        private void Update()
        {
            // Move bullet only if the direction has already been set
            if (_dir != Vector2.zero)
            {
                if (_initialPos == Vector2.zero)
                {
                    _initialPos = transform.position;
                }

                Move();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((type == BulletType.enemyBullet && collision.CompareTag("Enemy")) || (type == BulletType.playerBullet && collision.CompareTag("Player")))
                return;

            if (!collision.CompareTag("Bullet") && !collision.CompareTag("EnemyTrigger") && !collision.CompareTag("Door"))
            {
                if (collision.TryGetComponent(out IDamageable id))
                {
                    id.TakeDamage(_stats.Damage);
                    SoundManager.Instance.PlaySound(SoundType.EnemyHit, 0.5f);
                }
                else
                {
                    Instantiate(Assets.Instance.bulletSplash_1, transform.position, Quaternion.identity);
                }

                StartCoroutine(Co_DisableBullet(0f));
            }
        }

        private IEnumerator Co_DisableBullet(float time)
        {
            yield return new WaitForSeconds(time);

            ParticlesManager.Instance.CreateParticle(ParticleType.PlayerHit, transform.position, 0.5f, Quaternion.identity);
            _initialPos = Vector2.zero;

            if (gameObject.activeInHierarchy)
                gameObject.SetActive(false);
        }

        private void Move()
        {
            // Check if the bullet has moved its maximum range
            Vector2 l = _initialPos - new Vector2(transform.position.x, transform.position.y);

            // Refactorizar toda la lógica de las balas, porque esto es una chapuza
            if (type == BulletType.playerBullet)
            {
                if (Mathf.Abs(Mathf.Abs(l.magnitude)) >= _stats.Range)
                    StartCoroutine(Co_DisableBullet(0f));

                transform.position += _stats.Speed * Time.deltaTime * new Vector3(_dir.normalized.x, _dir.normalized.y);
            }
            else if (type == BulletType.enemyBullet)
            {
                if (l.magnitude >= enemyBulletRange)
                    StartCoroutine(Co_DisableBullet(0f));

                transform.position += enemyBulletMoveSpeed * Time.deltaTime * new Vector3(_dir.normalized.x, _dir.normalized.y);
            }

            Rotate(_dir);
        }

        private void Rotate(Vector3 dir) { transform.up = dir; }

        public void SetDirStatsAndColor(Vector2 dir, BulletStats stats, Color color)
        {
            _dir = dir;
            _stats = stats;
            sr.color = color;
        }
    }
}