using System.Collections;
using UnityEngine;

namespace PabloLario.Animations
{
    [System.Serializable]
    public class HitColorChangeAnimation
    {
        [SerializeField] private SpriteRenderer agentRenderer;

        public Color agentColor = new Color(1f,1f,1f);
        [SerializeField] private Color hitColor = new Color(1f,0f,0f);
        [SerializeField] private Color invencibilityColor = new Color(1f, 0f, 0f);
        [SerializeField] private float timeToWaitForColorChange = 0.05f;

        public IEnumerator Co_HitColorChange(bool makeInvencible, float invencibilityTime)
        {
            agentRenderer.color = hitColor;

            yield return new WaitForSeconds(timeToWaitForColorChange);

            agentRenderer.color = agentColor;

            if (makeInvencible)
                yield return SetInvencibilityColor(invencibilityTime);
        }

        private IEnumerator SetInvencibilityColor(float invencibilityTime)
        {
            agentRenderer.color = invencibilityColor;

            yield return new WaitForSeconds(invencibilityTime);

            agentRenderer.color = agentColor;
        }
    }
}


