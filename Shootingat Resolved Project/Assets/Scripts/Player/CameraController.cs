using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5;
    [SerializeField] private float zOffset = -10;
    [SerializeField] private float screenShakeTime = 0.2f;

    private float screenShakeTimeCounter;

    private void Update()
    {
        CalculateCamPos();
    }

    /// <summary>
    /// Calculates the camera position according to the player position and the mouse position.
    /// </summary>
    private void CalculateCamPos()
    {
        // I don't know if I should keep all the vector variables, because it is going to waste memory,
        // but the code is mucho more clear in this way. I guess that I am not going to change this code,
        // which means that it doesn't need to be that clear, as long as it saves resources from the system.

        Vector2 playerPos = Assets.Instance.playerTransform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = mousePos - playerPos;
        Vector2 newPos = playerPos + dir / 2;
        Vector2 smoothPos = Vector2.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothPos.x, smoothPos.y, zOffset);
    }

    /// <summary>
    /// Makes the camera to shake for a few miliseconds
    /// </summary>
    public IEnumerator ScreenShake()
    {
        screenShakeTimeCounter = screenShakeTime;

        while (screenShakeTimeCounter > 0f)
        {
            float newRandomX = Random.Range(-0.05f, 0.05f);
            float newRandomY = Random.Range(-0.05f, 0.05f);
            transform.position += new Vector3(newRandomX, newRandomY, zOffset);
            screenShakeTimeCounter -= Time.deltaTime;
            yield return null;
        }
    }
}
