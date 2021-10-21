namespace PabloLario.Managers
{ 
    public static class GameManager
    {
        public delegate void DungeonGenerated();
        public static event DungeonGenerated OnDungeonGenerated;

        public static void InvokeDelegateDungeonGeneration()
        {
            OnDungeonGenerated?.Invoke();
        }

        public delegate void EnemyDead(int abilityPoints);
        public static event EnemyDead OnEnemyDead;

        public static void InvokeDelegateEnemyDead(int abilityPoints)
        {
            OnEnemyDead?.Invoke(abilityPoints);
        }
    }
}
