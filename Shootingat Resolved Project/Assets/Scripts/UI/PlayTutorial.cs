using PabloLario.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTutorial : MonoBehaviour, ISelectable
{
    public void SelectButton()
    {
        Assets.Instance.playerTransform.position = new Vector3(0f, 0f);
        SceneManager.LoadScene(2);
    }
}
