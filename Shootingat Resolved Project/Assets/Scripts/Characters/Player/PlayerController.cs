using UnityEngine;
using UnityEngine.SceneManagement;
using PabloLario.Characters.Player.Powerups;
using PabloLario.Characters.Core.Shooting;
using PabloLario.Managers;

#pragma warning disable CS0414 // Quitar miembros privados no leídos

namespace PabloLario.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private MainCameraController camController;
        [SerializeField] private Transform weaponTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerStats ps;
        [SerializeField] private BoxCollider2D playerCollider;
        [SerializeField] private GameObject walkParticles;

        [Header("Fields")]
        [SerializeField] private float dashTime;
        [SerializeField] private float dashSpeedUpperLimit;
        [SerializeField] private float dashDropRate;

        private float _dashTimeCounter;
        private float _horizontal;
        private float _vertical;
        private float _fireRateCounter;
        private Vector3 _mousePos;
        private bool _moving;
        private bool _shooting;
        private Vector2 _dir;
        private float _dashSpeedSmoothed;

        private Camera _camera;
        private Transform _transform;

        private void Awake()
        {
            _camera = Camera.main;
            _transform = transform;
        }

        private void Start()
        {
            // fireRateCounter = ps.FireRate;
            _fireRateCounter = ps.fireRate.Value;

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
            camController = FindObjectOfType<MainCameraController>();
            transform.position = Vector3.zero;
        }

        private void Update()
        {
            _fireRateCounter += Time.deltaTime;

            CheckDash();

            // TODO: Refactor this into its own method
            if (_dashTimeCounter > 0f)
            {
                _transform.position += new Vector3(_horizontal, _vertical).normalized * _dashSpeedSmoothed * Time.deltaTime;
                _dashTimeCounter -= Time.deltaTime;
                _dashSpeedSmoothed -= ps.moveSpeed.Value / dashDropRate;
                _dashSpeedSmoothed = Mathf.Clamp(_dashSpeedSmoothed, 0f, dashSpeedUpperLimit);
                return;
            }

            Move();

            Rotate();

            CheckShoot();

            CheckShootFinish();
        }

        private void Move()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");

            if (_horizontal != 0f || _vertical != 0f)
            {
                _moving = true;
                ActivateWalkParticles();
                SoundManager.Instance.PlaySound(SoundType.PlayerWalk, 1.5f);
            }
            else
            {
                _moving = false;
                DeactivateWalkParticles();
            }

            _transform.position += new Vector3(_horizontal, _vertical).normalized * ps.moveSpeed.Value * Time.deltaTime;
        }

        private void Rotate()
        {
            _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            _dir = _mousePos - transform.position;
            weaponTransform.up = _dir;

            Animate(_dir);
        }

        private void CheckShoot()
        {
            if (Input.GetMouseButton(0))
            {
                if (_fireRateCounter < ps.fireRate.Value)
                    return;

                Shoot();
            }
        }

        private void Shoot()
        {
            _shooting = true;
            _fireRateCounter = 0f;

            GameObject go = BulletPoolManager.Instance.RequestPlayerBullet();
            go.transform.position = shootPoint.position;
            go.transform.rotation = Quaternion.Euler(shootPoint.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z/* + Random.Range(-10f, 5f)*/);

            Bullet b = go.GetComponent<Bullet>();
            b.SetDirAndStats(_dir, ps.bulletStats);

            StartCoroutine(camController.ScreenShake());

            ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);
            SoundManager.Instance.PlaySound(SoundType.PlayerShoot, 1f);
        }

        private void CheckShootFinish()
        {
            if (_fireRateCounter >= ps.fireRate.Value)
            {
                _shooting = false;
            }
        }

        private void CheckDash()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_dashTimeCounter > 0f)
                    return;

                Dash();
            }
        }

        private void Dash()
        {
            SoundManager.Instance.PlaySound(SoundType.PlayerDash, 1f);
            _dashTimeCounter = dashTime;
            animator.SetTrigger("Dashing");
            _dashSpeedSmoothed = dashSpeedUpperLimit;
        }

        private void Animate(Vector2 dir)
        {
            animator.SetBool("Moving", _moving);
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
