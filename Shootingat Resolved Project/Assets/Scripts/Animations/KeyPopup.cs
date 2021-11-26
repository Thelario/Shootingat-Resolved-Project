using UnityEngine;

public class KeyPopup : MonoBehaviour
{
    [SerializeField] private float longTime;
    [SerializeField] private float shortTime;
    
    [SerializeField] private Vector2 initialScale;
    [SerializeField] private Vector2 finalScale;
    [SerializeField] private Vector2 increasedScale;

    public void EnableKeyPopup()
    {
        Enable();
        AnimateToIncreased(true);
    }

    public void DisableKeyPopup()
    {
        AnimateToIncreased(false);
    }

    private void AnimateToIncreased(bool goToFinal)
    {
        if (goToFinal)
            LeanTween.scale(gameObject, increasedScale, longTime).setOnComplete(AnimateToBackToFinal);
        else
            LeanTween.scale(gameObject, increasedScale, shortTime).setOnComplete(AnimateToInitial);
    }

    private void AnimateToBackToFinal()
    {
        LeanTween.scale(gameObject, finalScale, shortTime);
    }

    private void AnimateToInitial()
    {
        LeanTween.scale(gameObject, initialScale, longTime).setOnComplete(Disable);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void Enable()
    {
        gameObject.SetActive(true);
    }
}
