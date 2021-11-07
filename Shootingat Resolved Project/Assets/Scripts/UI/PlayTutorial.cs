using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTutorial : MonoBehaviour, ISelectable
{
    public void SelectButton()
    {
        SceneManager.LoadScene(2);
    }
}
