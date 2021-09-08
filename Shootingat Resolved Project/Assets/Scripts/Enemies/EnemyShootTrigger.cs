using UnityEngine;

public class EnemyShootTrigger : MonoBehaviour
{
    [SerializeField] private EnemyShootBasic esb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            esb.playerOnZone = true;
            esb.target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            esb.playerOnZone = true;
            esb.target = collision.transform;
        }
    }
}
