using UnityEngine;
using PabloLario.Managers;

namespace PabloLario.Characters.Player
{
    public class MinimapCameraController : Singleton<MinimapCameraController>
    {
        [Header("Camera Values")]
        [SerializeField] private float smoothSpeed = 5;
        [SerializeField] private float zOffset = -10;
        [SerializeField] private float minimumCameraSize;
        [SerializeField] private float maximumCameraSize;
        [SerializeField] private float cameraZoomSpeed;

        [Header("Input Names")]
        [SerializeField] private string mouseWheelInputName;

        private Camera mainCamera;
        private Camera minimapCamera;

        protected override void Awake()
        {
            base.Awake();
            
            mainCamera = Camera.main;
            minimapCamera = GetComponent<Camera>();
        }

        private void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        private void Update()
        {
            CalculateCamPos();

            CalculateCamSize();
        }

        private void CalculateCamPos()
        {
            if (Assets.Instance.playerTransform == null)
                Assets.Instance.playerTransform = FindObjectOfType<PlayerController>().transform;

            Vector2 playerPos = Assets.Instance.playerTransform.position;
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - playerPos;
            Vector2 newPos = playerPos + dir / 2;
            Vector2 smoothPos = Vector2.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);

            transform.position = new Vector3(smoothPos.x, smoothPos.y, zOffset);
        }

        private void CalculateCamSize()
        {
            float mouseWheel = -Input.GetAxis(mouseWheelInputName);
            float newCameraSize = minimapCamera.orthographicSize + mouseWheel * cameraZoomSpeed * Time.deltaTime;
            minimapCamera.orthographicSize = Mathf.Clamp(newCameraSize, minimumCameraSize, maximumCameraSize);
        }
    }
}
