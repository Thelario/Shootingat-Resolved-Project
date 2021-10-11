using UnityEngine;

namespace PabloLario.Characters.Player
{
    public class Minimap : MonoBehaviour
    {
        [Header("Minimap Camera Reference")]
        [SerializeField] private Camera _minimapCamera;

        [Header("Minimap Scale and Pos")]
        [SerializeField] private Vector3 _defaultMinimapPos;
        [SerializeField] private Vector3 _defaultMinimapScale;
        [SerializeField] private Vector3 _openMinimapPos;
        [SerializeField] private Vector3 _openMinimapScale;

        private bool _defaultValuesPlaced = true;

        private RectTransform _rt;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                SetOpenedValues();
                return;
            }

            if (_defaultValuesPlaced)
                return;

            SetDefaultValues();
        }

        private void SetOpenedValues()
        {
            _defaultValuesPlaced = false;
            _rt.anchoredPosition = _openMinimapPos;
            _rt.localScale = _openMinimapScale;
        }

        private void SetDefaultValues()
        {
            _defaultValuesPlaced = true;
            _rt.anchoredPosition = _defaultMinimapPos;
            _rt.localScale = _defaultMinimapScale;
        }
    }
}
