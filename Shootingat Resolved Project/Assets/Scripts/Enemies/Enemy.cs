using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : HealthAgent
{
    public delegate void EnemyDead(int clarityEarned);
    public static EnemyDead OnEnemyDead;

    [SerializeField] protected int clarityToGiveToPlayerWhenDied;

    public Room RoomAssociatedTo { get; set; }
}
