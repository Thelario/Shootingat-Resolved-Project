using PabloLario.Characters.Core.Shooting;
using PabloLario.Characters.Player.Powerups;
using PabloLario.Managers;
using PabloLario.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable CS0414 //Quitar miembros privados no leídos

namespace PabloLario.Characters.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("References")]
        [SerializeField] private Transform shootPoint;
        [SerializeField] private MainCameraController camController;
        [SerializeField] private Transform weaponTransform;
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerStats ps;
        [SerializeField] private CircleCollider2D playerCollider;
        [SerializeField] private GameObject walkParticles;
        [SerializeField] private Popup weaponPopup;
        [SerializeField] private Image abilityImage;

        [Header("Fields")]
        [SerializeField] private float dashTime;
        [SerializeField] private float dashSpeedUpperLimit;
        [SerializeField] private float dashDropRate;
        [SerializeField] private bool playDashSound = true;
        [SerializeField] private float timeBetweenDashes = 1f;

        private float _dashTimeCounter;
        private float _timeBetweenDashesCounter;
        private float _horizontal;
        private float _vertical;
        private float _fireRateCounter;
        private Vector3 _mousePos;
        private bool _moving;
        private bool _shooting;
        private float _dashSpeedSmoothed;
        private bool _pause;

        private Rigidbody2D _rb;
        private Camera _camera;

        [HideInInspector] public Ability currentAbility;
        [HideInInspector] public Vector2 dir;

        protected override void Awake()
        {
            base.Awake();
            
            _camera = Camera.main;
        }

        private void Start()
        {
            _fireRateCounter = ps.fireRate.Value;
            _timeBetweenDashesCounter = 0f;

            if (_camera == null)
                _camera = Camera.main;

            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelLoaded;
            GameManager.OnPauseGame += Pause;
            GameManager.OnUnPauseGame += UnPause;
            GameManager.OnDungeonGenerated += SetAbilityToDefault;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelLoaded;
            GameManager.OnPauseGame -= Pause;
            GameManager.OnUnPauseGame -= UnPause;
            GameManager.OnDungeonGenerated -= SetAbilityToDefault;
        }

        private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            camController = FindObjectOfType<MainCameraController>();
            if (scene.buildIndex == 0)
            {
                transform.position = new Vector3(0f, 0f);
                camController.transform.position = new Vector3(0f, 0f);
            }
        }

        private void Update()
        {
            if (_pause)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
                CanvasManager.Instance.SwitchCanvas(CanvasType.PauseGameMenu, false);
            
            _fireRateCounter += Time.deltaTime;
            _timeBetweenDashesCounter -= Time.deltaTime;
            if (_timeBetweenDashesCounter <= -.5f)
                _timeBetweenDashesCounter = -.5f;

            CheckDash();

            if (IsDashing())
                return;

            CheckAbilityUse();
            
            Move();

            Rotate();

            CheckShoot();

            CheckShootFinish();
        }

        private void FixedUpdate()
        {
            if (Dash())
                return;

            if (Input.GetKey(KeyCode.LeftShift))
                _rb.MovePosition(_rb.position + ((ps.moveSpeed.Value / 2f) * Time.fixedDeltaTime * new Vector2(_horizontal, _vertical).normalized));
            else
                _rb.MovePosition(_rb.position + (ps.moveSpeed.Value * Time.fixedDeltaTime * new Vector2(_horizontal, _vertical).normalized));
        }

        private void SetAbilityToDefault()
        {
            if (currentAbility != null)
                currentAbility.DestroyAbilityObjectInstantly();

            abilityImage.sprite = null;
            currentAbility = null;
            abilityImage.color = new Color(abilityImage.color.r, abilityImage.color.g, abilityImage.color.b, 0f);
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
            go.transform.SetPositionAndRotation(shootPoint.position,
                Quaternion.Euler(shootPoint.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y,
                    shootPoint.rotation.eulerAngles.z + Random.Range(-10f, 5f)));

            Bullet b = go.GetComponent<Bullet>();
            b.SetDirStatsColor(dir, ps.bulletStats, ps.hitAnimation.agentColor);

            weaponPopup.AnimatePopup();
            StartCoroutine(camController.ScreenShake());

            animator.SetTrigger("Shooting");
            ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f,
                shootPoint.rotation);
            SoundManager.Instance.PlaySound(SoundType.PlayerShoot, 1f);
        }

        private void CheckAbilityUse()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (currentAbility != null)
                    currentAbility.UseAbility(ps, this);
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

                if (_timeBetweenDashesCounter > 0f)
                    return;
                
                StartDash();
            }
        }

        private void StartDash()
        {
            if (playDashSound)
                SoundManager.Instance.PlaySound(SoundType.PlayerDash, 1f);

            _dashTimeCounter = dashTime;
            AnimateDash();
            _dashSpeedSmoothed = dashSpeedUpperLimit;
            _timeBetweenDashesCounter = timeBetweenDashes;
        }

        private bool Dash()
        {
            if (IsDashing())
            {
                _rb.MovePosition(_rb.position + (new Vector2(_horizontal, _vertical).normalized * _dashSpeedSmoothed * Time.fixedDeltaTime));

                _dashTimeCounter -= Time.fixedDeltaTime;
                _dashSpeedSmoothed -= ps.moveSpeed.Value / dashDropRate;
                _dashSpeedSmoothed = Mathf.Clamp(_dashSpeedSmoothed, 0f, dashSpeedUpperLimit);
                return true;
            }

            return false;
        }

        private bool IsDashing()
        {
            if (_dashTimeCounter <= 0f)
                transform.localScale = new Vector3(1f, 1f);
                
            return _dashTimeCounter > 0f;
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
            transform.localScale = new Vector3(0.75f, 0.75f);
        }

        private void ActivateWalkParticles()
        {
            walkParticles.SetActive(true);
        }

        private void DeactivateWalkParticles()
        {
            //walkParticles.SetActive(false);
        }

        private void Pause()
        {
            _pause = true;
        }

        private void UnPause()
        {
            _pause = false;
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
                Powerup[] powerups = collision.gameObject.GetComponents<Powerup>();
                foreach (Powerup powerup in powerups)
                    powerup.ApplyPowerup(ps);
            }
            else if (collision.gameObject.TryGetComponent(out Ability a))
            {
                if (currentAbility != null)
                    Destroy(currentAbility.gameObject);

                currentAbility = a;
                a.transform.parent = transform;
                SoundManager.Instance.PlaySound(SoundType.PickPowerup);

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
