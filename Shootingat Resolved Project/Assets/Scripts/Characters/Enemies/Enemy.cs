using UnityEngine;
using PabloLario.DungeonGeneration;

namespace PabloLario.Characters.Enemies
{
    public abstract class Enemy : HealthAgent, IRoomAssignable
    {
        public delegate void EnemyDead(int clarityEarned);
        public static EnemyDead OnEnemyDead;

        [SerializeField] protected int clarityToGiveToPlayerWhenDied;

        protected Room _roomAssociatedTo;

        public void AssignRoom(Room room)
        {
            _roomAssociatedTo = room;
        }
    }
}