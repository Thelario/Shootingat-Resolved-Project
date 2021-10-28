using UnityEngine;

namespace PabloLario.Characters.Core.Shooting
{
    public class TemporalShield : MonoBehaviour
    {
        [SerializeField] private string cutoffAnimatorName;

        private SpriteRenderer _sr;
        private Animator _animator;

        private float _destroyTime;
        private float _destroyTimeCounter = -1f;
        private bool _destroyAbility;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_destroyTimeCounter < 0f || !_destroyAbility)
                return;

            _destroyTimeCounter += Time.deltaTime;
            if (_destroyTimeCounter >= _destroyTime)
            {
                Destroy(gameObject, 1.5f);
                _animator.SetTrigger(cutoffAnimatorName);
            }
        }

        public void SetColorAndDestroyTime(Color c, float destroyTime, bool destroyAbility)
        {
            _sr.color = c;
            _destroyTime = destroyTime;
            _destroyAbility = destroyAbility;
            _destroyTimeCounter = 0f;
        }
    }
}
