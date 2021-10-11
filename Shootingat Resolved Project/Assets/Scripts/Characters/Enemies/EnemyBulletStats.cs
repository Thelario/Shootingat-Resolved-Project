using PabloLario.Characters.Core.Shooting;
using UnityEngine;

namespace PabloLario.Characters.Enemies
{
    [System.Serializable]
    public class EnemyBulletStats : BulletStats
    {
        [SerializeField]
        private int _damage;

        public override int Damage { get => _damage; set => _damage = value; }

        [SerializeField]
        private float _range;
        public override float Range { get => _range; set => _range = value; }

        [SerializeField]
        private float _speed;
        public override float Speed { get => _speed; set => base.Speed = value; }

    }

}
