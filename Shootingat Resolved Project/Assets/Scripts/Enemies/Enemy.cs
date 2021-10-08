using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : HealthAgent, IRoomAssignable
{
    public delegate void EnemyDead(int clarityEarned);
    public static EnemyDead OnEnemyDead;

    [SerializeField] protected int clarityToGiveToPlayerWhenDied;

    protected Room _roomAssociatedTo;

    public void AssignRoom(Room room)
    {
        _roomAssociatedTo = room;
    }
}
