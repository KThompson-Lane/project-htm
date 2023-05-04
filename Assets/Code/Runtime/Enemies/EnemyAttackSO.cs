using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackSO", menuName = "ScriptableObject/EnemyAttack")]
public class EnemyAttackSO : ScriptableObject
{
    public bool ranged = false;
    public int damage = 1;
    public float range = 0f; //todo - fire range
    public float rateOfFire = 120f;
    public float bulletForce = 5f;
    public GameObject projectile;
}
