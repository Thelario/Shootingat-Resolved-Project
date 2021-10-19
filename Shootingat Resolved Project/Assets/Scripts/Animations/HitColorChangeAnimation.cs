using System.Collections;
using UnityEngine;

namespace PabloLario.Animations
{
    [System.Serializable]
    public class HitColorChangeAnimation
    {
        [SerializeField] private SpriteRenderer agentRenderer;

        public Color agentColor = new Color(1f,1f,1f);
        [SerializeField] private Color hitColor = new Color(1f,0,0);

        [SerializeField] private float timeToWaitForColorChange = 0.05f;

        public IEnumerator Co_HitColorChange()
        {
            agentRenderer.color = hitColor;

            yield return new WaitForSeconds(timeToWaitForColorChange);

            agentRenderer.color = agentColor;
        }
    }
}


