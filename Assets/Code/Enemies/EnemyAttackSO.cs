using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "ScriptableObject/EnemyAttack")]
public class EnemyAttackSO : ScriptableObject
{
    public bool ranged = false;
    public float damage = 1f;
    public float range = 0f;
    public float rateOfFire = 120f;
    public GameObject projectile;
}
