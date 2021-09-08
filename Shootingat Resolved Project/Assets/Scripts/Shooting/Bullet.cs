using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletType type; // Type of the bullet
    [SerializeField] private float enemyBulletMoveSpeed;
    [SerializeField] private float enemyBulletRange;

    private Vector2 dir = Vector2.zero; // Direction to point to
    private Vector2 initialPos = Vector2.zero;

    private PlayerStats ps;

    private void Awake()
    {
        ps = PlayerStats.Instance;
    }

    private void LateUpdate()
    {
        // Move bullet only if the direction has already been set
        if (dir != Vector2.zero)
        {
            if (initialPos == Vector2.zero)
            {
                initialPos = transform.position;
            }

            Move();
        }
    }

    /*
    private void OnEnable()
    {
        // When a bullet is activated, it needs to start counting for the time to deactivate again
        StartCoroutine(DisableBullet(ps.bulletRange));
    }
    */

    /// <summary>
    /// Method for handling the collision of a bullet with enemies.
    /// </summary>
    /// <param name="collision"> Collision2D component of the object the bullet has collided with </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == BulletType.enemyBullet && collision.CompareTag("Enemy"))
            return;

        if (!collision.CompareTag("Bullet") && !collision.CompareTag("EnemyTrigger") && !collision.CompareTag("Door"))
        {
            if (collision.TryGetComponent(out IDamageable id))
            {
                id.TakeDamage(ps.bulletDamage);
                SoundManager.Instance.PlaySound(SoundType.EnemyHit, 0.5f);
            }
            else
            {
                Destroy(Instantiate(Assets.Instance.bulletSplash_1, transform.position, Quaternion.identity), 2f);
            }

            StartCoroutine(DisableBullet(0f));
        }
    }

    /// <summary>
    /// Method for Deactivating the bullet according to the time passed as parameter.
    /// </summary>
    /// <param name="time"> Maximum time the bullet can be travelling activated </param>
    private IEnumerator DisableBullet(float time)
    {
        yield return new WaitForSeconds(time);

        ParticlesManager.Instance.CreateParticle(ParticleType.PlayerHit, transform.position, 0.5f, Quaternion.identity);
        initialPos = Vector2.zero;

        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// Moves bullet with a certain speed in the dir direction
    /// </summary>
    private void Move()
    {
        // Refactorizar toda la l�gica de las balas, porque esto es una chapuza
        if (type == BulletType.playerBullet)
        {
            // Check if the bullet has moved its maximum range
            Vector2 l = initialPos - new Vector2(transform.position.x, transform.position.y);
            if (Mathf.Abs(Mathf.Abs(l.magnitude)) >= ps.bulletRange)
                StartCoroutine(DisableBullet(0f));

            transform.position += new Vector3(dir.normalized.x, dir.normalized.y) * ps.bulletSpeed * Time.deltaTime;
        }
        else if (type == BulletType.enemyBullet)
        {
            // Check if the bullet has moved its maximum range
            Vector2 l = initialPos - new Vector2(transform.position.x, transform.position.y);
            if (l.magnitude >= enemyBulletRange)
                StartCoroutine(DisableBullet(0f));

            transform.position += new Vector3(dir.normalized.x, dir.normalized.y) * enemyBulletMoveSpeed * Time.deltaTime;
        }

        Rotate(dir);
    }

    /// <summary>
    /// Rotates bullet so that it points to dir
    /// </summary>
    /// <param name="dir"> Direction to point to </param>
    private void Rotate(Vector3 dir) { transform.up = dir; }

    /// <summary>
    /// Sets the bullet direction to rotate and move to dir
    /// </summary>
    /// <param name="dir"> Direction to point to </param>
    public void SetDir(Vector2 dir) { this.dir = dir; }
}