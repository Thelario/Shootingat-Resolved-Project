using UnityEngine;

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

        public delegate void GameState();
        public static event GameState OnPauseGame;
        public static event GameState OnUnPauseGame;
        public static event GameState OnWinGame;
        public static event GameState OnLostGame;

        public static void InvokeDelegateOnPauseGame()
        {
            Time.timeScale = 0f;
            OnPauseGame?.Invoke();
            //Debug.Log(Time.timeScale);
        }

        public static void InvokeDelegateOnUnPauseGame()
        {
            Time.timeScale = 1f;
            OnUnPauseGame?.Invoke();
            //Debug.Log(Time.timeScale);
        }

        public static void InvokeDelegateOnWinGame()
        {
            OnWinGame?.Invoke();
        }

        public static void InvokeDelegateOnLostGame()
        {
            OnLostGame?.Invoke();
        }
    }
}
