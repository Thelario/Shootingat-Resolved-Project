using UnityEngine;
using UnityEngine.SceneManagement;
using PabloLario.Powerups;
using PabloLario.Shooting;
using PabloLario.Managers;
// using Random = UnityEngine.Random; // Un-comment in case I add using System

#pragma warning disable CS0414 // Quitar miembros privados no le�dos

namespace PabloLario.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private CameraController camController;
        [SerializeField] private Transform weaponTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerStats ps;
        [SerializeField] private BoxCollider2D playerCollider;
        [SerializeField] private GameObject walkParticles;

        [Header("Fields")]
        [SerializeField] private float dashTime;
        [SerializeField] private float dashSpeedUpperLimit;
        [SerializeField] private float dashDropRate;

        private float dashTimeCounter;
        private float horizontal;
        private float vertical;
        private float fireRateCounter;
        private Vector3 mousePos;
        private bool moving;
        private bool shooting;
        private Vector2 dir;
        private float dashSpeedSmoothed;

        private Camera _camera;
        private Transform _transform;

        private void Awake()
        {
            _camera = Camera.main;
            _transform = transform;
        }

        private void Start()
        {
            fireRateCounter = ps.fireRate;

            if (_camera == null)
                _camera = Camera.main;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelLoaded;
        }

        private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            camController = FindObjectOfType<CameraController>();
            transform.position = Vector3.zero;
        }

        private void Update()
        {
            fireRateCounter += Time.deltaTime;

            CheckDash();

            // TODO: Refactor this into its own method
            if (dashTimeCounter > 0f)
            {
                _transform.position += new Vector3(horizontal, vertical).normalized * dashSpeedSmoothed * Time.deltaTime;
                dashTimeCounter -= Time.deltaTime;
                dashSpeedSmoothed -= ps.moveSpeed / dashDropRate;
                dashSpeedSmoothed = Mathf.Clamp(dashSpeedSmoothed, 0f, dashSpeedUpperLimit);
                return;
            }

            Move();

            Rotate();

            CheckShoot();

            CheckShootFinish();
        }

        private void Move()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0f || vertical != 0f)
            {
                moving = true;
                ActivateWalkParticles();
                SoundManager.Instance.PlaySound(SoundType.PlayerWalk, 1.5f);
            }
            else
            {
                moving = false;
                DeactivateWalkParticles();
            }

            _transform.position += new Vector3(horizontal, vertical).normalized * ps.moveSpeed * Time.deltaTime;
        }

        private void Rotate()
        {
            mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            dir = mousePos - transform.position;
            weaponTransform.up = dir;

            Animate(dir);
        }

        private void CheckShoot()
        {
            if (Input.GetMouseButton(0))
            {
                if (fireRateCounter < ps.fireRate)
                    return;

                Shoot();
            }
        }

        private void Shoot()
        {
            shooting = true;
            fireRateCounter = 0f;

            GameObject go = BulletPoolManager.Instance.RequestPlayerBullet();
            go.transform.position = shootPoint.position;
            go.transform.rotation = Quaternion.Euler(shootPoint.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z/* + Random.Range(-10f, 5f)*/);

            Bullet b = go.GetComponent<Bullet>();
            b.SetDir(dir);

            StartCoroutine(camController.ScreenShake());

            ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);
            SoundManager.Instance.PlaySound(SoundType.PlayerShoot, 1f);
        }

        private void CheckShootFinish()
        {
            if (fireRateCounter >= ps.fireRate)
            {
                shooting = false;
            }
        }

        private void CheckDash()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashTimeCounter > 0f)
                    return;

                Dash();
            }
        }

        private void Dash()
        {
            SoundManager.Instance.PlaySound(SoundType.PlayerDash, 1f);
            dashTimeCounter = dashTime;
            animator.SetTrigger("Dashing");
            dashSpeedSmoothed = dashSpeedUpperLimit;
        }

        private void Animate(Vector2 dir)
        {
            animator.SetBool("Moving", moving);
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
            //animator.SetBool("Shooting", shooting);
        }

        private void ActivateWalkParticles()
        {
            walkParticles.SetActive(true);
        }

        private void DeactivateWalkParticles()
        {
            walkParticles.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If collided with a powerup, then take it
            if (collision.gameObject.TryGetComponent(out Powerup p))
            {
                p.ApplyPowerup(ps);
            }
        }
    }
}
