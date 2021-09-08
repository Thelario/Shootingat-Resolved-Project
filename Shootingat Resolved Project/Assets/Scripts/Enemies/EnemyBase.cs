using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : HealthAgent
{
    public delegate void EnemyDead(int clarityEarned);
    public static EnemyDead OnEnemyDead;

    [SerializeField] protected int clarityToGiveToPlayerWhenDied;
}
