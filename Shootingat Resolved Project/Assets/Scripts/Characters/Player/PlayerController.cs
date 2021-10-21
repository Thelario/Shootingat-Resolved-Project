using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PabloLario.Characters.Player.Powerups;
using PabloLario.Characters.Core.Shooting;
using PabloLario.Managers;
using PabloLario.UI;

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
        [SerializeField] private Popup weaponPopup;
        [SerializeField] private Image abilityImage;

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

        private float _dashSpeedSmoothed;

        private Camera _camera;
        private Transform _transform;

        public Ability _currentAbility;

        public Vector2 dir;

        private void Awake()
        {
            _camera = Camera.main;
            _transform = transform;
        }

        private void Start()
        {
            _fireRateCounter = ps.fireRate.Value;

            if (_camera == null)
                _camera = Camera.main;

            Time.timeScale = 1f;
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

            if (Dash())
                return;

            CheckAbilityUse();

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

            _transform.position += ps.moveSpeed.Value * Time.deltaTime * new Vector3(_horizontal, _vertical).normalized;
        }

        private void Rotate()
        {
            _mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            dir = _mousePos - transform.position;
            weaponTransform.up = dir;

            Animate(dir);
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
            go.transform.SetPositionAndRotation(shootPoint.position, Quaternion.Euler(shootPoint.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z + Random.Range(-10f, 5f)));

            Bullet b = go.GetComponent<Bullet>();
            b.SetDirStatsColor(dir, ps.bulletStats, ps.hitAnimation.agentColor);

            weaponPopup.AnimateScorePopup();
            StartCoroutine(camController.ScreenShake());

            animator.SetTrigger("Shooting");
            ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);
            SoundManager.Instance.PlaySound(SoundType.PlayerShoot, 1f);
        }

        private void CheckAbilityUse()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_currentAbility != null)
                    _currentAbility.UseAbility(ps, this);
            }
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

                StartDash();
            }
        }

        private void StartDash()
        {
            SoundManager.Instance.PlaySound(SoundType.PlayerDash, 1f);
            _dashTimeCounter = dashTime;
            AnimateDash();
            _dashSpeedSmoothed = dashSpeedUpperLimit;
        }

        private bool Dash()
        {
            if (_dashTimeCounter > 0f)
            {
                _transform.position += new Vector3(_horizontal, _vertical).normalized * _dashSpeedSmoothed * Time.deltaTime;
                _dashTimeCounter -= Time.deltaTime;
                _dashSpeedSmoothed -= ps.moveSpeed.Value / dashDropRate;
                _dashSpeedSmoothed = Mathf.Clamp(_dashSpeedSmoothed, 0f, dashSpeedUpperLimit);
                return true;
            }

            return false;
        }

        private void Animate(Vector2 dir)
        {
            if (!animator.enabled)
                return;

            animator.SetBool("Moving", _moving);
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
        }

        private void AnimateDash()
        {
            if (!animator.enabled)
                return;

            animator.SetTrigger("Dashing");
        }

        private void ActivateWalkParticles()
        {
            walkParticles.SetActive(true);
        }

        private void DeactivateWalkParticles()
        {
            //walkParticles.SetActive(false);
        }

        public Transform GetShootPointTransform()
        {
            return shootPoint;
        }

        public Transform GetWeaponTransform()
        {
            return weaponTransform;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Portal"))
            {
                TeleportManager.Instance.PlayerOnTeleport = true;
            }
            else if (collision.gameObject.TryGetComponent(out Powerup p))
            {
                p.ApplyPowerup(ps);
            }
            else if (collision.gameObject.TryGetComponent(out Ability a))
            {
                if (_currentAbility != null)
                    Destroy(_currentAbility.gameObject);

                _currentAbility = a;

                // TODO: extract responsability from here and move it to somewhere, like a UI manager or smth
                abilityImage.sprite = a.abilitySprite;
                abilityImage.color = new Color(abilityImage.color.r, abilityImage.color.g, abilityImage.color.b, 1f);

                a.gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Portal"))
            {
                TeleportManager.Instance.PlayerOnTeleport = false;
            }
        }
    }
}
