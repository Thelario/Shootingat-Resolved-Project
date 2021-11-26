using System.Collections;
using PabloLario.Characters.Core.Stats;
using PabloLario.Managers;
using UnityEngine;
using TMPro;

namespace PabloLario.Characters.Core.Shooting
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletType type; // Type of the bullet
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private TrailRenderer tr;

        private Vector2 _dir = Vector2.zero; // Direction to point to
        private Vector2 _initialPos = Vector2.zero;

        protected BulletStats _stats;

        protected virtual void Update()
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
            if (collision.CompareTag("Shield"))
            {
                if (type == BulletType.PlayerBullet || type == BulletType.GigantBullet)
                    return;

                StartCoroutine(Co_DisableBullet(0f));
            }
            else
            {
                if ((type == BulletType.EnemyBullet && collision.CompareTag("Enemy")) || (type == BulletType.PlayerBullet && collision.CompareTag("Player")) || type == BulletType.GigantBullet)
                    return;

                if (collision.CompareTag("Bullet") || collision.CompareTag("EnemyTrigger") || collision.CompareTag("Door"))
                    return;

                if (collision.TryGetComponent(out IDamageable id))
                {
                    id.TakeDamage(_stats.Damage);
                    SoundManager.Instance.PlaySound(SoundType.EnemyHit, 0.5f);
                    GameObject damageFloatingText = Instantiate(Assets.Instance.damageFloatingText, collision.transform.position + Vector3.up, Quaternion.identity);
                    damageFloatingText.GetComponent<TMP_Text>().text = _stats.Damage.ToString();
                    
                    Destroy(damageFloatingText, 2f);
                }
                else
                {
                    GameObject splash = Instantiate(Assets.Instance.bulletSplash1, transform.position, Quaternion.identity);
                    splash.GetComponent<SpriteRenderer>().color = sr.color;
                }

                StartCoroutine(Co_DisableBullet(0f));
            }
        }

        public IEnumerator Co_DisableBullet(float time)
        {
            yield return new WaitForSeconds(time);

            tr.Clear();
            ParticlesManager.Instance.CreateParticle(ParticleType.PlayerHit, transform.position, 0.5f, Quaternion.identity);
            _initialPos = Vector2.zero;

            if (gameObject.activeInHierarchy)
                gameObject.SetActive(false);
        }

        private void Move()
        {
            // Check if the bullet has moved its maximum range
            Vector2 l = _initialPos - new Vector2(transform.position.x, transform.position.y);

            if (Mathf.Abs(Mathf.Abs(l.magnitude)) >= _stats.Range)
                StartCoroutine(Co_DisableBullet(0f));

            transform.position += _stats.Speed * Time.deltaTime * new Vector3(_dir.normalized.x, _dir.normalized.y);

            Rotate(_dir);
        }

        private void Rotate(Vector3 dir) { transform.up = dir; }

        public void SetDirStatsColor(Vector2 dir, BulletStats stats, Color color)
        {
            _dir = dir;
            _stats = stats;

            if (type == BulletType.GigantBullet)
                _stats.Range *= 2;

            sr.color = color;
            tr.colorGradient = SetTrailColor(color);
        }

        private Gradient SetTrailColor(Color c)
        {
            Gradient g = new Gradient();

            GradientColorKey[] gcks = new GradientColorKey[2];
            gcks[0] = new GradientColorKey(c, 10f);
            gcks[1] = new GradientColorKey(c, 10f);

            GradientAlphaKey[] gaks = new GradientAlphaKey[2];
            gaks[0] = new GradientAlphaKey(1f, 10f);
            gaks[1] = new GradientAlphaKey(1f, 10f);

            g.SetKeys(gcks, gaks);
            return g;
        }
    }
}