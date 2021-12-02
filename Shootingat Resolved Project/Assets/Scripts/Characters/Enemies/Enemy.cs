using UnityEngine;
using PabloLario.DungeonGeneration;

namespace PabloLario.Characters.Enemies
{
    public abstract class Enemy : HealthAgent, IRoomAssignable
    {
        public delegate void EnemyDead(int abilityPointsearned);
        public static EnemyDead OnEnemyDead;

        [SerializeField] protected int abilityPointsToGiveToPlayerWhenDied;

        protected Room _roomAssociatedTo;

        public void AssignRoom(Room room)
        {
            _roomAssociatedTo = room;
        }

        private void OnDisable()
        {
            print("Enabling enemy after being disabled when spawnned");
            gameObject.SetActive(true);
        }
    }
}