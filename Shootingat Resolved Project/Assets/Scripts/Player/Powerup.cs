using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    [SerializeField] protected bool goodPowerup = true;

    public abstract void ApplyPowerup();
}
