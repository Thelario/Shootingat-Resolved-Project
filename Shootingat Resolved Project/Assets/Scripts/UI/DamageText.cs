using UnityEngine;

namespace PabloLario.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private float timeToScaleDown;
        [SerializeField] private float timeToMoveUp;
        [SerializeField] private float directionMultiplier;

        private void Start()
        {
            ScaleDown();
            MoveUp();
        }

        private void ScaleDown()
        {
            LeanTween.scale(gameObject, Vector3.zero, timeToScaleDown);
        }

        private void MoveUp()
        {
            float r = Random.Range(-0.25f, 0.25f);
            if (Mathf.Abs(r) < 0.1f) 
                r = 0f;

            Vector3 dirToMove = new Vector3(r, 1f);
            LeanTween.move(gameObject, transform.position + dirToMove * directionMultiplier, timeToMoveUp);
        }
    }
}