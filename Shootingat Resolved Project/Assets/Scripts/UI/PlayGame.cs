using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour, ISelectable
{
    public void SelectButton()
    {
        SceneManager.LoadScene(1);
    }
}
