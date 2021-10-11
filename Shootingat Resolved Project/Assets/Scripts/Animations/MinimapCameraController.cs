using UnityEngine;
using PabloLario.Managers;

namespace PabloLario.Player
{
    public class MinimapCameraController : MonoBehaviour
    {
        [Header("Camera Values")]
        [SerializeField] private float smoothSpeed = 5;
        [SerializeField] private float zOffset = -10;
        [SerializeField] private float _minimumCameraSize;
        [SerializeField] private float _maximumCameraSize;
        [SerializeField] private float _cameraZoomSpeed;

        [Header("Input Names")]
        [SerializeField] private string _mouseWheelInputName;

        private Camera _mainCamera;
        private Camera _minimapCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _minimapCamera = GetComponent<Camera>();
        }

        private void Start()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
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
            Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - playerPos;
            Vector2 newPos = playerPos + dir / 2;
            Vector2 smoothPos = Vector2.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);

            transform.position = new Vector3(smoothPos.x, smoothPos.y, zOffset);
        }

        private void CalculateCamSize()
        {
            float mouseWheel = -Input.GetAxis(_mouseWheelInputName);
            float newCameraSize = _minimapCamera.orthographicSize + mouseWheel * _cameraZoomSpeed * Time.deltaTime;
            _minimapCamera.orthographicSize = Mathf.Clamp(newCameraSize, _minimumCameraSize, _maximumCameraSize);
        }
    }
}
