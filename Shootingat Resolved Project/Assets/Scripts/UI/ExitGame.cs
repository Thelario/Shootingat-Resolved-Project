using UnityEngine;

public class ExitGame : MonoBehaviour, ISelectable
{
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SelectButton()
    {
        Exit();
    }
}