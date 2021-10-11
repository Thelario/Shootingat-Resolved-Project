using PabloLario.Managers;
using System.Collections;
using UnityEngine;

namespace PabloLario.Player
{
    public class MainCameraController : Singleton<MainCameraController>
    {
        [SerializeField] private float smoothSpeed = 5;
        [SerializeField] private float zOffset = -10;
        [SerializeField] private float screenShakeTime = 0.2f;

        private float screenShakeTimeCounter;

        private Camera _camera;

        protected override void Awake()
        {
            base.Awake();

            _camera = Camera.main;
        }

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;
        }

        private void Update()
        {
            CalculateCamPos();
        }

        private void CalculateCamPos()
        {
            if (Assets.Instance.playerTransform == null)
                Assets.Instance.playerTransform = FindObjectOfType<PlayerController>().transform;

            Vector2 playerPos = Assets.Instance.playerTransform.position;
            Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - playerPos;
            Vector2 newPos = playerPos + dir / 2;
            Vector2 smoothPos = Vector2.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);

            transform.position = new Vector3(smoothPos.x, smoothPos.y, zOffset);
        }

        public IEnumerator ScreenShake()
        {
            screenShakeTimeCounter = screenShakeTime;

            while (screenShakeTimeCounter > 0f)
            {
                float newRandomX = Random.Range(-0.05f, 0.05f);
                float newRandomY = Random.Range(-0.05f, 0.05f);

                transform.position += new Vector3(newRandomX, newRandomY, zOffset);
                screenShakeTimeCounter -= Time.deltaTime;

                yield return null;
            }
        }
    }
}