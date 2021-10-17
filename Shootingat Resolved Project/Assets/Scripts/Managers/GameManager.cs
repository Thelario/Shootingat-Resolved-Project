namespace PabloLario
{ 
    public static class GameManager
    {
        public delegate void DungeonGenerated();
        public static event DungeonGenerated OnDungeonGenerated;

        public static void InvokeDelegate()
        {
            OnDungeonGenerated?.Invoke();
        }
    }
}
