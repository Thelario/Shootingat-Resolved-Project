using UnityEngine;
using PabloLario.Managers;

public class PortalButton : MonoBehaviour
{
    private Transform _portalTransform;

    private void Awake()
    {
        _portalTransform = transform.parent.parent;
    }

    public void ClickPortalButton()
    {
        TeleportManager.Instance.TeleportPlayer(_portalTransform.position);
    }
}
