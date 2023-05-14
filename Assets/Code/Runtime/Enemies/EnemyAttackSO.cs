using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "ScriptableObject/EnemyAttack")]
public abstract class EnemyAttackSO : ScriptableObject
{
    public int damage = 1;
    public float range = 0f; //todo - fire range
    public float rateOfFire = 120f;
}
